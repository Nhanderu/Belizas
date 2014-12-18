using System;

namespace Nhanderu.TheRealTable.CsvData
{
    interface ICsvTable
    {
        Boolean Validate();

        void Read(String data);

        void ReadFile(String path);

        void WriteFile(String path);

        String ToString();
    }
}
