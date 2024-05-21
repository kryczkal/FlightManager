namespace projob.DataBaseSQL;

public class DeleteCommand : FunctionCommand
{
    public DeleteCommand()
    {
        // The main function deletes passed objects
        Function = (dataBaseObjects, args) =>
        {
            var baseObjects = dataBaseObjects.ToList();
            foreach (var dataBaseObject in baseObjects)
            {
                if (Settings.SQLSettings.CascadeDelete) DataBaseManager.CascadeDeleteById(dataBaseObject.Id);
                else DataBaseManager.DeleteById(dataBaseObject.Id);
            }
            return baseObjects;
        };
    }
}
