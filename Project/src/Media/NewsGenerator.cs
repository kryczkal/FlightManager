namespace projob.media;

public class NewsGenerator
{
    private List<Media> MediaList { get; }
    private List<IReportable> ReportableList { get;}

    public NewsGenerator(List<Media> mediaList, List<IReportable> reportableList)
    {
        MediaList = mediaList;
        ReportableList = reportableList;
    }

    public IEnumerable<string> GenerateNextNews()
    {
        foreach (var media in MediaList)
        {
            foreach (var reportable in ReportableList)
            {
                yield return reportable.AcceptMediaReport(media);
            }
        }
    }
}