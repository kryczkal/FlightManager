namespace projob.DataBaseSQL;
public class DisplayCommand : FunctionCommand
{
    public DisplayCommand(List<string> objectProperties)
    {
        // The main function displays passed objects via logger as an aesthetically formatted table
        Function = (dataBaseObjects, args) =>
        {
            var baseObjects = dataBaseObjects.ToList();

            // Check if all objectProperties are valid
            foreach (var objectProperty in objectProperties)
            {
                if (objectProperty != "*" && !baseObjects.First().Accessors.ContainsKey(objectProperty))
                {
                    Console.WriteLine($"Property {objectProperty} does not exist in the object", LogLevel.Error);
                    return baseObjects;
                }
            }

            // Handle the case where there are no objects to display
            if (baseObjects.Count == 0)
            {
                Console.WriteLine("No objects to display", LogLevel.Info);
                return baseObjects;
            }

            // Handle the case where the user wants to display all properties
            if (objectProperties.Count == 1)
            {
                if (objectProperties[0] == "*")
                {
                    objectProperties = baseObjects.First().Accessors.Keys.ToList();

                }
            }

            // Find the widest string in each column for formatting
            List<int> widest = new List<int>(objectProperties.Count);
            for (int i = 0; i < objectProperties.Count; i++)
            {
                widest.Add(objectProperties[i].Length);
            }

            // Iterate over all objects and properties to find the widest string
            foreach (var dataBaseObject in baseObjects)
            {
                for (int i = 0; i < objectProperties.Count; i++)
                {
                    int propertyLength = dataBaseObject.Accessors[objectProperties[i]].Get().Length;
                    if (propertyLength > widest[i])
                    {
                        widest[i] = propertyLength;
                    }
                }
            }

            // Ensure the header is included in the widest length calculations
            for (int i = 0; i < objectProperties.Count; i++)
            {
                if (objectProperties[i].Length > widest[i])
                {
                    widest[i] = objectProperties[i].Length;
                }
            }

            // Add padding to the column widths
            for (int i = 0; i < widest.Count; i++)
            {
                widest[i] += 2; // Add padding of 1 space on each side
            }

            // Create String.Format templates for the header, separator, and row
            string headerTemplate = "|";
            string rowTemplate = "|";
            string separator = "+";

            for (int i = 0; i < objectProperties.Count; i++)
            {
                headerTemplate += $" {{{i},-{widest[i]}}} |"; // Left-align the headers
                rowTemplate += $" {{{i},{widest[i]}:>}} |"; // Right-align the values
                separator += new string('-', widest[i] + 2) + "+";
            }

            Console.WriteLine(String.Format(headerTemplate, objectProperties.ToArray()), LogLevel.Info);

            // Log the separator row
            Console.WriteLine(separator, LogLevel.Info);

            // Create and log a row for each object
            foreach (var dataBaseObject in baseObjects)
            {
                List<string> row = new List<string>();
                foreach (var property in objectProperties)
                {
                    row.Add(" " + dataBaseObject.Accessors[property].Get() + " ");
                }

                Console.WriteLine(String.Format(rowTemplate, row.ToArray()), LogLevel.Info);
            }

            return baseObjects;
        };


    }

}
