using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    public class Entry
    {
        public Entry(string name, Entry? parent, bool isDir)
        {
            Name = name;
            Parent = parent;
            IsDir = isDir;
            Entries = new List<Entry>();
        }

        public string Name { get; private set; }
        public Entry? Parent { get; private set; }
        public bool IsDir { get; private set; }
        public long Size { get; set; }
        public void AddSize(long size)
        {
            Size += size;
            if (Parent != null)
            {
                Parent.AddSize(size);
            }
        }
        public List<Entry> Entries { get; private set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, IsDir: {1}, Parent: {2}, Size: {3}", Name, IsDir ? "true" : "false", Parent == null ? "null" : Parent.Name, Size);
        }
    }
}
