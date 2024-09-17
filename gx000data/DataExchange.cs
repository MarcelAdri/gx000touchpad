namespace gx000data;

public class DataExchange
{
    public enum DataStatus
    {
        Synchronized = 0,
        FromSimToClient = 1,
        FromClientToSim = 2,
    }
    
    public static bool StatusChangeIsOK(DataStatus dataStatus, DataStatus newStatus)
    {
        //TODO
        throw new NotImplementedException();
    }
}