using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffGridConsole.Objects
{
    internal class Profiles
    {
        public List<Profile> ProfileList { get; set; }

        public Profiles() 
        {
            this.ProfileList = new List<Profile>();
        }

        public void AppendList(Profile pf)
        {
            ProfileList.Add(pf);
        }

        public void ReadData()
        {
            int db = 0;
            foreach (Profile pf in ProfileList)
            {
                Console.Write(db + ". profile: ");
                pf.ReadProfileData();
                pf.ReadConsumptions();
                db++;
            }
        }

    }
}
