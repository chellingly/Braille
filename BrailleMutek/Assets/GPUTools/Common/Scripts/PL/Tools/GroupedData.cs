using System.Collections.Generic;

namespace GPUTools.Common.Scripts.PL.Tools
{
    public interface IGroupItem
    {
        bool HasConflict(IGroupItem item);
    }

    public struct GroupData
    {
        public int Start;
        public int Num;

        public GroupData(int start, int num)
        {
            Start = start;
            Num = num;
        }
    }

    public class GroupedData<T> where T:IGroupItem //todo inherit from GpuBuffer
    {
        public List<GroupData> GroupsData = new List<GroupData>();

        public List<List<T>> Groups = new List<List<T>>();

        public void AddGroup(List<T> list)
        {
            Groups.Add(list);
        }

        public void Add(T item)
        {
            for (int j = 0; j < Groups.Count; j++)
            {
                var items = Groups[j];
                var hasConflicts = false;

                for (int i = 0; i < items.Count; i++)
                {
                    var testItem = items[i];
                    if (item.HasConflict(testItem))
                    {
                        hasConflicts = true;
                        break;
                    }
                }

                if (!hasConflicts)
                {
                    items.Add(item);
                    return;
                }
            }

            var list = new List<T> {item};
            Groups.Add(list);
        }

        public T[] Data
        {
            get
            {
                var data = new List<T>();

                foreach (var items in Groups)
                {
                    GroupsData.Add(new GroupData(data.Count, items.Count));
                    data.AddRange(items);
                }

                return data.ToArray();//todo something wrong
            }
        }

        public void Dispose()
        {
            //dispose buffer here
        }
    }
}
