namespace ApplesGame
{
    public class ApplesGameConfig
    {
        private string username;
        private int treesCount;
        private int applesOnTreeCount;

        public ApplesGameConfig()
        {

        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int TreesCount
        {
            get { return this.treesCount; }
            set { this.treesCount = value; }
        }

        public int ApplesOnTreeCount
        {
            get { return this.applesOnTreeCount; }
            set { this.applesOnTreeCount = value; }
        }
    }
}
