using Store444.Models;

namespace Store444.Dopomoga;

public static class EnumHelper
{
    public static string GetStatus(Status status)
    {
        switch (status)
        {
            case Status.OnTheWay:
            return "В дорозі";

            case Status.Delivered:
            return "Доставлено";

            case Status.Cancelled:
            return "Скасовано";

            case Status.Packaging:
            return "В центрі пакування";

            default:
            throw new ArgumentException("Неіснуючий статус!");
        }
    }
}
