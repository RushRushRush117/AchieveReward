using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchieveReward
{
    public enum AchievementGoalType
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Year,
        NoLimit
    }
    class AchievementGoalModel
    {
        private List<AchievementItem> mItems;
        public string mDesc;
        private AchievementGoalType mType;

        public void AddItem(AchievementItem item)
        {
            if (mItems == null) { mItems = new List<AchievementItem>(); }
            else { mItems.Add(item); }
        }
    }
}
