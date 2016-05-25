namespace MongoCrud
{
    public class MCollection
    {
        public string Name { get; set; }
        public string[] Indices { get; set; }

        public MCollection(string name)
        {
            this.Name = name;
            this.Indices = new string[] {};
        }

        public MCollection(string name, string[] indices )
        {
            this.Name = name;
            this.Indices = indices;
        }
    }
}
