using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_DashboardModel
    {
        public EmailActivity emailActivity;
        public ActiveUsers activeUsers;
        public ActivationCount activationCount;
        public SharePointActivity sharepointActivity;
        public SkypeActivity skypeActivity;
        public TeamsActivity teamsActivity;
        public OneDriveUsage onedriveUsage;

        public List<string> Metrics;
        public List<int> ChartType;
        public List<GraphData> GraphData;
    }

    public class GraphData
    {
        public string[] Legends { get; set; }
        public string[] Props { get; set; }
        public List<List<string>> Values { get; set; }
        public OtherProperties Others { get; set; }
    }

    public class OtherProperties
    {
        public double Total { get; set; }
        public double Trend { get; set; }
        public int SortOrder { get; set; }
        public string Url { get; set; }
        public string imageUrl { get; set; }
        public string ToolTip { get; set; }
    }

    public class EmailActivity
    {
        public int Send { get; set; }
        public int Recived { get; set; }
        public int Read { get; set; }
        public int Total { get; set; }
        public double Trend { get; set; }
    }
    public class ActiveUsers
    {
        public int Office365 { get; set; }
        public int Exchange { get; set; }
        public int OneDrive { get; set; }
        public int SharePoint { get; set; }
        public int SkypeForBusiness { get; set; }
        public int Yammer { get; set; }
        public int Teams { get; set; }
    }
    public class ActivationCount
    {
        public int Desktop { get; set; }
        public int Devices { get; set; }
    }
    public class SharePointActivity
    {
        public int ViewedOrEdited { get; set; }
        public int Synced { get; set; }
        public int SharedInternally { get; set; }
        public int SharedExternally { get; set; }
        public int Total { get; set; }
        public double Trend { get; set; }
    }
    public class SkypeActivity
    {
        public int PeerToPeer { get; set; }
        public int Organized { get; set; }
        public int Participated { get; set; }
        public int Total { get; set; }
        public double Trend { get; set; }
    }
    public class TeamsActivity
    {
        public int ChatMessages { get; set; }
        public int ChannelMessages { get; set; }
        public int Total { get; set; }
        public double Trend { get; set; }
    }

    public class OneDriveUsage
    {
        public List<OneDriveMetric> Usages { get; set; }
        public string Total { get; set; }
        public double Trend { get; set; }
    }

    public class OneDriveMetric
    {
        public string Date { get; set; }
        public string StorageUsed { get; set; }
    }
}
