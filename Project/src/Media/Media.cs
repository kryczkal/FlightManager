using Products;

namespace projob.media;

public interface IReportable
{
    public string AcceptMediaReport(Media media);
}
public abstract class Media
{
    public abstract string ReportAirport(Airport airport);
    public abstract string ReportCargoPlane(CargoPlane cargoPlane);
    public abstract string ReportPassengerPlane(PassengerPlane passengerPlane);
}

public class Television : Media
{
    public override string ReportAirport(Airport airport)
    {
        string report = $"<An image of {airport.Name} airport>";
        return report;
    }

    public override string ReportCargoPlane(CargoPlane cargoPlane)
    {
        string report = $"<An image of {cargoPlane.Name} cargo plane>";
        return report;
    }

    public override string ReportPassengerPlane(PassengerPlane passengerPlane)
    {
        string report = $"<An image of {passengerPlane.Name} passenger plane>";
        return report;
    }
}
public class Radio : Media
{
    private string RadioName { get; set; }

    public Radio(string radioName)
    {
        RadioName = radioName;
    }

    public override string ReportAirport(Airport airport)
    {
        string report = $"Reporting for {RadioName}, Ladies and gentelmen, we are at the {airport.Name} airport";
        return report;
    }

    public override string ReportCargoPlane(CargoPlane cargoPlane)
    {
        string report = $"Reporting for {RadioName}, Ladies and gentelmen, we are seeing the {cargoPlane.Serial} aircraft fly above us";
        return report;
    }

    public override string ReportPassengerPlane(PassengerPlane passengerPlane)
    {
        string report = $"Reporting for {RadioName}, Ladies and gentelmen, we've just witnessed {passengerPlane.Serial} take off";
        return report;
    }
}
public class Newspaper : Media
{
    private string NewspaperName { get; set; }
    
    public Newspaper(string newspaperName)
    {
        NewspaperName = newspaperName;
    }


    public override string ReportAirport(Airport airport)
    {
        string report = $"{NewspaperName} - A report from the {airport.Name} airport, {airport.ISOCountryCode}";
        return report;
    }

    public override string ReportCargoPlane(CargoPlane cargoPlane)
    {
        string report = $"{NewspaperName} - An interview with the crew of {cargoPlane.Serial}";
        return report;
    }

    public override string ReportPassengerPlane(PassengerPlane passengerPlane)
    {
        string report = $"{NewspaperName} - Breaking news! {passengerPlane.Name} aircraft loses EASA fails certification after inspection of {passengerPlane.Serial}";
        return report;
    }
}