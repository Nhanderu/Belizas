using System;
using System.Web;
using Nhanderu.TheRealTable.CsvData;

namespace Nhanderu.TheRealTable
{
    interface ITable
    {
        IHtmlString ToHtml();

        CsvTable ToCsv();

        Object ToJson();
    }
}
