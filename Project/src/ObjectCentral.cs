using DataTransformation;

namespace projob;

public static class ObjectCentral
{
    public static GeneralUtils.ConcurrentList<IDataTransformable> Objects = new();

    public static void MakeSnapshot(ISerializer serializer)
    {
        Console.WriteLine("Printing data...");
        string filePath = "assets/snapshot_" + DateTime.Now.ToString("HH_mm_ss") + "."+ serializer.GetFormat();
        DataTransformationUtils.SerializeObjListToFile(Objects.GetList() , filePath, serializer);
    }
}