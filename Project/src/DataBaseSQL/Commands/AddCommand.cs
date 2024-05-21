using Products;

namespace projob.DataBaseSQL;

public class AddCommand : FunctionCommand
{
    public AddCommand(Tuple<string, string>[] keyValuePairs)
    {
        Function = (dataBaseObjects, args) =>
        {
            DataBaseObjectFactory factory = new();
            foreach (string arg in args)
            {
                var newObject = factory.CreateProduct(arg);
                if (newObject == null) continue;
                foreach (var keyValPair in keyValuePairs)
                {
                    var (key, value) = keyValPair;
                    newObject.Accessors[key].Set(value);
                }
                DataBaseManager.AddObj(newObject);
                dataBaseObjects.Concat([newObject]);
            }

            return dataBaseObjects;
        };
    }
}