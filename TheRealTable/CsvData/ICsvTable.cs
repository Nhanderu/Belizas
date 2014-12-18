using System;

namespace Nhanderu.TheRealTable.CsvData
{
    interface ICsvTable
    {
        Boolean Validate();

        void Read();

        void ReadFile();

        void WriteFile();

        String ToString();
    }
}
