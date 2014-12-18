using System;
using System.Collections.Generic;

namespace Nhanderu.TheRealTable.CsvData
{
    class CsvTable : ICsvTable
    {
        private List<List<String>> _data;

        public bool Validate()
        {
            throw new NotImplementedException();
        }

        public void Read(String data)
        {
            throw new NotImplementedException();
        }

        public void ReadFile(String path)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(String path)
        {
            throw new NotImplementedException();
        }
    }
}
