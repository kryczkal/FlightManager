namespace projob.DataBaseSQL;

public class UpdateCommand : FunctionCommand
{
    public UpdateCommand(Tuple<string, string>[] keyValPairs)
    {
        Function = (dataBaseObjects, args) =>
        {
            var baseObjects = dataBaseObjects.ToList();
            foreach (var dataBaseObject in baseObjects)
            {
                foreach (var keyValPair in keyValPairs)
                {
                    dataBaseObject.Accessors[keyValPair.Item1].Set(keyValPair.Item2);
                }
            }
            return baseObjects;
        };
    }


}