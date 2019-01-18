using System;
using System.Linq;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Database.Utilities
{
    public class CatalogUtilities
    {
        private readonly CldpDbContext _dbContext;

        public CatalogUtilities(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedCategoriesProductsAndProductItems()
        {
            #region ProductCategories
            var serviceSuitesCategory = new ProductCategory
            {
                Name = "Service Suites"
            };
            
            _dbContext.ProductCategories.Add(serviceSuitesCategory);

            _dbContext.SaveChanges();
            #endregion
            #region Products
            var office365Product = new Product
            {
                Name = "Office 365",
                Category = serviceSuitesCategory,
                Description = @"Office 365 refers to subscription plans that include access to Office applications plus other productivity services that are enabled over the internet(cloud services). Office 365 includes plans for use at home and for business. Learn about Office for use at home. Office 365 plans for business include services such as Skype for Business web conferencing and Exchange Online hosted email for business and additional online storage with OneDrive for Business.",
                Vendor = "Microsoft Corporation",
                ImgUrl = "https://rickzeleznik.files.wordpress.com/2014/04/officelogoorange_print.png"
            };
            _dbContext.Products.Add(office365Product);

            _dbContext.SaveChanges();
            #endregion
            #region ProductItems
            var office365BusinessEssentialsProductItem = new ProductItem
            {
                Name = "Office 365 Business Essentials",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 0,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Exchange.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SharePoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SkypeForBusiness.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Teams.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Yammer.png' width='30' height='30' /></div><div class='main-description'><div>Online versions of Office with email, instant messaging, HD video conferencing, plus 1 TB personal file storage and sharing. Does not include Office suite for PC or Mac. For organization with up to 300 users.</div><br /><div>Office 365 Business Essentials provides easy online access to industry-leading messaging, collaboration, and storage solutions. With Business Essentials you can send and receive email, communicate with co-workers, access your files, create and edit documents, and much more all through a web browser from any computer. The service includes:</div><ul class='included-services'><li>Business Exchange E-mail: A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs and can be accessed using an Outlook Desktop client, ActiveSync client on your mobile device, or the web based interface using your favorite browser.</li><li>OneDrive File storage and sharing: OneDrive provides a 1 TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Skype for Business: Communication software that allows you to stay connected with your origination members using instant messaging, video conferencing, and file sharing.</li><li>Microsoft Teams: Communicate with your team using an Office 365 integrates software that allows you to stay connected to your origination’s members in real time by integrating all your users, content, and tools.</li><li>Yammer: A collaborative software designed for business application that allows your organization to stay connected to origination teams such as technical support teams, marketing, and sales teams collaborating on various projects.</li><li>SharePoint and Team Sites: Organize your company’s document resources using SharePoint Online to share, organize, and collaborate on a wide verity of data such as news, social media, and resource application. Content can be shared with anyone within and outside your organization.</li><li>Full Office Online: Take advantage of the full online versions of Word, Excel, PowerPoint and Outlook, accessible via your favorite web browser. Business Essentials does not include the Office software suite for your PC, Mac, or mobile device.</li><li>Sway: New to the Office 365 Apps offering, allows you to create newsletters, reports and interactive presentations from your favorite web browser.</li><li>Planner: Provides easy team collaboration to create plans, stay organized with assigned task and update task progress in real time.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "3B555118-DA6A-4418-894F-7DF1E2096870"
            };

            var office365BusinessProductItem = new ProductItem
            {
                Name = "Office 365 Business",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 1,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Outlook.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Word.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Excel.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerPoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneNote.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Access.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /></div><div class='main-description'><div>The Office suite for PC and Mac with apps for tablets and phones, plus 1 TB personal file storage and sharing. Does not include email service. For organizations with up to 300 users.</div><br /><div>Office 365 Business gives you the latest Office applications for your PC, Mac, and mobile device. Create, edit, and share your ideas using all of your devices, online or offline. With the multi-platform Office suite, each user can install the same applications on up to 5 PC or Macs, 5 tablets, and 5 phones. Access to the online versions of the Office suite via your favorite web browser is also included, allowing you to work anywhere on any platform. The service includes:</div><ul class='included-services'><li>The Full Office Application Suite: Take advantage of the full Office suit that includes Microsoft Word, Excel, PowerPoint, Outlook, OneNote, Publisher, and Access.</li><li>Office Online: Work with Word, Excel, PowerPoint, and OneNote documents from any computer using your favorite web browser.</li><li>OneDrive File storage and sharing: OneDrive provides a 1 TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "CDD28E44-67E3-425E-BE4C-737FAB2899D3"
            };

            var office365BusinessPremiumProductItem = new ProductItem
            {
                Name = "Office 365 Business Premium",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 2,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Outlook.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Word.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Excel.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerPoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneNote.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Access.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Exchange.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SharePoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SkypeForBusiness.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Teams.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Yammer.png' width='30' height='30' /></div><div class='main-description'><div>The Office suite for PC and Mac with apps for tablets and phones, plus email, instant messaging, HD video conferencing, 1 TB personal file storage and sharing. For organizations with up to 300 users.</div><br /><div>Office 365 Business Premium provides all of the features of both the Office 365 Business and Business Essentials services in one convenient package. Communicate, collaborate, and access your documents from any computer or device through the full Office application suite or online portal through your web browser. The service includes:</div><ul class='included-services'><li>The Full Office Application Suite: Take advantage of the full Office suit that includes Microsoft Word, Excel, PowerPoint, Outlook, OneNote, Publisher, and Access.</li><li>Office Online: Work with Word, Excel, PowerPoint, and OneNote documents from any computer using your favorite web browser.</li><li>Business Exchange E-mail: A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using either an Outlook Desktop client or the web based interface using your favorite browser. For more info click here.</li><li>OneDrive File storage and sharing: OneDrive provides a 1TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Skype for Business: Communication software that allows you to stay connected with your origination members using instant messaging, video conferencing, and file sharing.</li><li>Yammer: Collaborative software designed for business application that allows your organization to stay connected to origination teams such as technical support teams, marketing, and sales teams collaborating on various projects.</li><li>SharePoint and Team Sites: Organize your company’s document resources using SharePoint Online to share, organize, and collaborate on a wide verity of data such as news, social media, and resource application. Content can be shared with anyone within and outside your organization.</li><li>Microsoft StaffHub: A scheduling software that connects your employees to view their schedules, swap shifts, request time off, and find additional information on team events.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "F245ECC8-75AF-4F8E-B61F-27D8114DE5F3"
            };

            var office365ProPlusProductItem = new ProductItem
            {
                Name = "Office 365 ProPlus",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 3,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Outlook.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Word.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Excel.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerPoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneNote.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Access.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /></div><div class='main-description'><div>The premium Office suite for organizations - including Word, Excel, PowerPoint, Outlook, OneNote, Access, and Skype for Business - plus online file storage and sharing. Connected to the cloud, enabling you to roam between your devices of choice as part of the Office 365 experience.</div><br /><div>Office 365 ProPlus gives you the latest Office applications for your PC, Mac, and mobile device. Create, edit, and share your ideas using all of your devices, online or offline. With the multi-platform Office suite, each user can install the same applications on up to 5 PC or Macs, 5 tablets, and 5 phones. Access to the online versions of the Office suite via your favorite web browser is also included, allowing you to work anywhere on any platform. The service includes:</div><ul class='included-services'><li>The Full Office Application Suite: Take advantage of the full Office suit that includes Microsoft Word, Excel, PowerPoint, Outlook, OneNote, and Access.</li><li>Office Online: Work with Word, Excel, and PowerPoint documents from any computer using your favorite web browser.</li><li>OneDrive File storage and sharing: OneDrive provides a 1 TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "C2273BD0-DFF7-4215-9EF5-2C7BCFB06425"
            };

            var office365EnterpriseE1ProductItem = new ProductItem
            {
                Name = "Office 365 Enterprise E1",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 4,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Exchange.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SharePoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SkypeForBusiness.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Teams.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Yammer.png' width='30' height='30' /></div><div class='main-description'><div>The online versions of Office with email, instant messaging, HD video conferencing, plus 1 TB personal file storage and sharing. Does not include the Office suite for PC or Mac.</div><br /><div>Office 365 Enterprise E1 provides easy online access to industry-leading messaging, collaboration, and storage solutions. With Business Essentials you can send and receive email, communicate with co-workers, access your files, create and edit documents, and much more all through a web browser from any computer. The service includes:</div><ul class='included-services'><li>Business Exchange E-mail: A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using either an Outlook Desktop client or the web based interface using your favorite browser.</li><li>SharePoint and Team Sites: Organize your company’s document resources using SharePoint Online to share, organize, and collaborate on a wide verity of data such as news, social media, and resource application. Content can be shared with anyone within and outside your organization.</li><li>Skype for Business: Communication software that allows you to stay connected with your origination members using instant messaging, video conferencing, and file sharing.</li><li>Microsoft Teams: Communicate with your team using an Office 365 integrates software that allows you to stay connected to your origination’s members in real time by integrating all your users, content, and tools.</li><li>OneDrive File storage and sharing: OneDrive provides a 1TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Yammer: Collaborative software designed for business application that allows your organization to stay connected to origination teams such as technical support teams, marketing, and sales teams collaborating on various projects.</li><li>Planner: Provides easy team collaboration to create plans, stay organized with assigned task and update task progress in real time.</li><li>Sway: New to the Office 365 Apps offering, allows you to create newsletters, reports, and interactive presentations from your favorite web browser.</li><li>Microsoft Stream allows you to create, discover, and share videos integrated with Office 365 apps across organizations.</li><li>Microsoft StaffHub: A scheduling software that connects your employees to view their schedules, swap shifts, request time off, and find additional information on team events.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "18181A46-0D4E-45CD-891E-60AABD171B4E"
            };

            var office365EnterpriseE3ProductItem = new ProductItem
            {
                Name = "Office 365 Enterprise E3",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 5,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Outlook.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Word.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Excel.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerPoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneNote.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Access.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Exchange.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SharePoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SkypeForBusiness.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Teams.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Yammer.png' width='30' height='30' /></div><div class='main-description'><div>The Office suite for PC and Mac with apps for tablets and phones, plus email, instant messaging, HD video conferencing, 1 TB personal file storage and sharing, and available add-ons like PSTN calling.</div><br /><div>Office 365 Enterprise E3 provides all of the features of both the Office 365 ProPlus and Enterprise E1 services, plus security and compliance tools such as legal hold, data loss prevention, and more. Communicate, collaborate, and access your documents from any computer or device through the full Office application suite or online portal through your web browser. The service includes:</div><ul class='included-services'><li>Full Office Online: Take advantage of the full Office 2016 suit that includes Microsoft Word, Excel, PowerPoint, Outlook, OneNote, and Access.</li><li>Business Exchange E-mail: A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using either an Outlook Desktop client or the web based interface using your favorite browser.</li><li>SharePoint and Team Sites: Organize your company’s document resources using SharePoint Online to share, organize, and collaborate on a wide verity of data such as news, social media, and resource application. Content can be shared with anyone within and outside your organization.</li><li>Message Archiving: Fully compliant arching solution that includes unlimited storage and legal hold abilities with full data loss prevention polices.</li><li>Skype for Business: Communication software that allows you to stay connected with your origination members using instant messaging, video conferencing, and file sharing.</li><li>Microsoft Teams: Communicate with your team using an Office 365 integrates software that allows you to stay connected to your origination’s members in real time by integrating all your users, content, and tools.</li><li>OneDrive File storage and sharing: OneDrive provides a 1TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Planner: Provides easy team collaboration to create plans, stay organized with assigned task and update task progress in real time.</li><li>Sway: New to the Office 365 Apps offering, allows you to create newsletters, reports, and interactive presentations from your favorite web browser.</li><li>Microsoft Stream allows you to create, discover, and share videos integrated with Office 365 apps across organizations.</li><li>Microsoft StaffHub: A scheduling software that connects your employees to view their schedules, swap shifts, request time off, and find additional information on team events.</li><li>Voicemail Integration: Hosted and fully Exchange Online integrated voicemail support that be accessed using Outlook or compatible mobile devices.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "6FD2C87F-B296-42F0-B197-1E91E994B900"
            };

            var office365EnterpriseE5ProductItem = new ProductItem
            {
                Name = "Office 365 Enterprise E5 (without PSTN Conferencing)",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 6,
                Product = office365Product,
                Description = @"<div style='margin-bottom: 1.25rem;'><img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Outlook.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Word.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Excel.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerPoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneNote.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Access.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Exchange.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/OneDrive.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SharePoint.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/SkypeForBusiness.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Teams.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/Yammer.png' width='30' height='30' /> <img style='margin-right: 0.3rem;' src='(#ImageServerOffice365IconsPath#)/PowerBI.png' width='30' height='30' /></div><div class='main-description'><div>The Office suite for PC and Mac, plus email, instant messaging, HD video conferencing, 1 TB personal file storage and sharing, analytics, and advanced security. Does not include PSTN conferencing.</div><br /><div>Office 365 Enterprise E5 provides all the features of Enterprise E3 plus advanced security, analytics, and voice features. Communicate, collaborate, and access your documents from any computer or device through the full Office application suite or online portal through your web browser. Manage your data and protect against accidental deletion or malware threats. Office 365 Enterprise E5 helps keep your business running reliably and efficiently. The service includes:</div><ul class='included-services'><li>Full Office Online: Take advantage of the full Office 2016 suit that includes Microsoft Word, Excel, PowerPoint, Outlook, OneNote, Publisher, and Access.</li><li>Business Exchange E-mail: A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using either an Outlook Desktop client or the web based interface using your favorite browser. For more info click here. (*Link to service offer)</li><li>SharePoint and Team Sites: Organize your company’s document resources using SharePoint Online to share, organize, and collaborate on a wide verity of data such as news, social media, and resource application. Content can be shared with anyone within and outside your organization.</li><li>Modern Voice with Phone System: VOIP service that allows you to receive, make, and transfer calls using your configured mobile device, Outlook PC client, or browser. This system also includes Audio Conferencing by dialing a single access number to connect to your team.</li><li>Message Archiving: Fully compliant arching solution that includes unlimited storage and legal hold abilities with full data loss prevention polices.</li><li>Advanced Security: Advanced E-mail Threat Protection defends against malicious content that can be found in E-mail attachments and links.</li><li>Skype for Business: Communication software that allows you to stay connected with your origination members using instant messaging, video conferencing, and file sharing.</li><li>Microsoft Teams: Communicate with your team using an Office 365 integrates software that allows you to stay connected to your origination’s members in real time by integrating all your users, content, and tools.</li><li>OneDrive File storage and sharing: OneDrive provides a 1TB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Planner: Provides easy team collaboration to create plans, stay organized with assigned task and update task progress in real time.</li><li>Yammer: Collaborative software designed for business application that allows your organization to stay connected to origination teams such as technical support teams, marketing, and sales teams collaborating on various projects.</li><li>Sway: New to the Office 365 Apps offering, allows you to create newsletters, reports, and interactive presentations from your favorite web browser.</li><li>Microsoft Stream allows you to create, discover, and share videos integrated with Office 365 apps across organizations.</li><li>Microsoft StaffHub: A scheduling software that connects your employees to view their schedules, swap shifts, request time off, and find additional information on team events.</li><li>Voicemail Integration: Hosted and fully Exchange Online integrated voicemail support that be accessed using Outlook or compatible mobile devices.</li></ul></div>
                              <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "26D45BD9-ADF1-46CD-A9E1-51E9A5524128"
            };

            var office365Plan1ProductItem = new ProductItem
            {
                Name = "Microsoft Exchange Online - Plan 1",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 7,
                Product = office365Product,
                Description = @"<div class='main-description'><div>Messaging, calendaring, and email archiving plan accessible from Outlook on PCs, the Web and mobile devices.</div></div><p> </p><div>Microsoft Exchange Online – Plan 1 provides a reliable, secure email service for users that already have their own applications or don't need anything more than email. The service includes:</div><ul class='included-services'><li>A 50GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using web based interface using your favorite browser. Includes personal archiving and anti-malware protection.</li></ul>
                                            <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "4B9405B0-7788-4568-ADD1-99614E613B69"
            };

            var office365Plan2ProductItem = new ProductItem
            {
                Name = "Microsoft Exchange Online - Plan 2",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 8,
                Product = office365Product,
                Description = @"<div class='main-description'><div>Best messaging and calendaring plan accessible from PCs, the Web and mobile devices with advanced archiving, compliance and integrated voicemail capabilities.</div></div><p> </p><div>Microsoft Exchange Online – Plan 2 provides a reliable, secure email service with unlimited storage and unified messaging for users that already have their own applications or just need hosted email. The service includes:</div><ul class='included-services'><li>A 100GB Exchange mailbox that can be used for all of your E-mail, contacts, and calendaring needs that can be accessed using web based interface using your favorite browser.</li><li>Unlimited storage for personal archiving as well as hosted Unified Messaging services.</li></ul>
                                            <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "19EC0D23-8335-4CBD-94AC-6050E30712FA"
            };

            var office365KioskProductItem = new ProductItem
            {
                Name = "Microsoft Exchange Online - Kiosk",
                BillingCycle = 1,
                BillingType = 1,
                Ord = 9,
                Product = office365Product,
                Description = @"<div class='main-description'><div>Basic messaging and calendaring plan with Web email and POP access.</div></div><p> </p><div>Microsoft Exchange Online – Kiosk provides email and collaborative services tailored for front desk customer service personnel. Easily manage your employees’ access, scheduling, tasks, and training. The service includes:</div><ul class='included-services'><li>Business Class E-mail: Get business-class email through a rich and familiar Outlook browser experience. Included is a 2 GB per user mailbox, attachments up to 150 MB, anti-malware protection, and anti-spam filtering. Exchange ActiveSync (EAS) is supported for phones and POP support for desktop email clients</li><li>Office Online: Take advantage of the Online Office suite that includes Microsoft Word, Excel, PowerPoint, and OneNote.</li><li>Microsoft StaffHub: A scheduling software that connects your employees to view their schedules, swap shifts, request time off, and find additional information on team events.</li><li>Yammer: Collaborative software designed for business application that allows your organization to stay connected to origination teams such as technical support teams, marketing, and sales teams collaborating on various projects.</li><li>Microsoft Teams: Communicate with your team using an Office 365 integrates software that allows you to stay connected to your origination’s members in real time by integrating all your users, content, and tools.</li><li>OneDrive File storage and sharing: One drive provides a 2GB personal storage space that can be accessed anywhere to quickly and easily edit or share your documents over the cloud.</li><li>Sway: New to the Office 365 Apps offering, allows you to create newsletters, reports, and interactive presentations from your favorite web browser.</li></ul>
                                            <style>.included-services {list-style-type: square;} .included-services li{margin-left: 1.25rem; margin-top: 0.625rem;}</style>",
                IsAddon = false,
                Identifier = "80B2D799-D2BA-4D2A-8842-FB0D0F3A4B82"
            };
            
            _dbContext.ProductItems.Add(office365BusinessEssentialsProductItem);
            _dbContext.ProductItems.Add(office365BusinessProductItem);
            _dbContext.ProductItems.Add(office365BusinessPremiumProductItem);
            _dbContext.ProductItems.Add(office365ProPlusProductItem);
            _dbContext.ProductItems.Add(office365EnterpriseE1ProductItem);
            _dbContext.ProductItems.Add(office365EnterpriseE3ProductItem);
            _dbContext.ProductItems.Add(office365EnterpriseE5ProductItem);
            _dbContext.ProductItems.Add(office365Plan1ProductItem);
            _dbContext.ProductItems.Add(office365Plan2ProductItem);
            _dbContext.ProductItems.Add(office365KioskProductItem);

            _dbContext.SaveChanges();
            #endregion
        }

        public void SeedCatalogs()
        {
            var allCompanies = _dbContext.Companies.ToList();
            var allProductItems = _dbContext.ProductItems.ToList();
            var rand = new Random();

            foreach (var company in allCompanies)
            {
                var defaultMarkup = 0.1m;

                Catalog randomParentCatalog;
                if (company.Parent != null)
                {
                    randomParentCatalog = company.Parent.CompanyCatalogs.Where(c=> c.CatalogType == CatalogType.MyCatalog).ElementAt(rand.Next(0,
                        company.Parent.CompanyCatalogs.Count - 1)).Catalog;

                    company.CompanyCatalogs.Add(new CompanyCatalog
                    {
                        Catalog = randomParentCatalog,
                        CatalogType = CatalogType.Assigned
                    });
                }
                else
                {
                    var topResellerAssignedCatalog = new Catalog
                    {
                        Name = "Top reseller catalog"
                    };
                    _dbContext.Catalogs.Add(topResellerAssignedCatalog);

                    foreach (var product in allProductItems)
                    {
                        var catalogProduct = new CatalogProductItem
                        {
                            ProductItem = product,
                            Catalog = topResellerAssignedCatalog,
                            ResellerPrice = Convert.ToDecimal(rand.Next(1, 10)),
                            RetailPrice = 0,
                            FixedRetailPrice = false,
                            Available = true
                        };
                        _dbContext.CatalogProductItems.Add(catalogProduct);
                    }

                    company.CompanyCatalogs.Add(new CompanyCatalog
                    {
                        Catalog = topResellerAssignedCatalog,
                        CatalogType = CatalogType.Assigned,
                    });
                    _dbContext.SaveChanges();

                    randomParentCatalog = topResellerAssignedCatalog;
                }

                var catalog = new Catalog
                {
                    Name = "Default Schedule"
                };
                _dbContext.Catalogs.Add(catalog);

                foreach (var product in allProductItems)
                {
                    decimal? resellerPrice = null;
                    decimal? retailPrice = null;
                    var parentCatalogProduct =
                        randomParentCatalog?.CatalogProductItems.FirstOrDefault(cp => cp.ProductItemId == product.Id);

                    if (parentCatalogProduct != null)
                    {
                        resellerPrice = parentCatalogProduct.ResellerPrice +
                                        parentCatalogProduct.ResellerPrice * defaultMarkup;
                        retailPrice = parentCatalogProduct.RetailPrice;
                    }

                    var catalogProduct = new CatalogProductItem
                    {
                        ProductItem = product,
                        Catalog = catalog,
                        ResellerPrice = resellerPrice ?? Convert.ToDecimal(rand.Next(1, 10)),
                        RetailPrice = retailPrice ?? Convert.ToDecimal(rand.Next(30, 90)),
                        FixedRetailPrice = true,
                        Available = true
                    };
                    _dbContext.CatalogProductItems.Add(catalogProduct);
                }

                company.CompanyCatalogs.Add(new CompanyCatalog
                {
                    Catalog = catalog,
                    CatalogType = CatalogType.MyCatalog,
                    Default = true
                });
            }

            _dbContext.SaveChanges();
        }
        
    }
}
