using System.Collections.Generic;
using CloudPlus.Entities;
using CloudPlus.Enums.Notification;

namespace CloudPlus.Database.Utilities
{
    public class EmailTemplatesUtilities
    {
        private readonly CldpDbContext _dbContext;

        public EmailTemplatesUtilities(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EmailTemplatesUtilities SeedEmailTemplates()
        {
            var emailTemplates = new List<EmailTemplate>
            {
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeUserPasswordViaEmail.ToString(),
                    Subject = "WELCOME – User Setup",
                    Template = @"
                                <p><span style='font-size: large;'><strong>WELCOME &ndash; User Setup</strong></span></p>
                                <p><br /> Welcome (#UserDisplayName#). You now have a user account at (#CompanyName#). Below you will find important details to assist you in accessing your account.</p>
                                <p>&nbsp;</p>
                                <p><span style='color: #ff0000;'><strong>IMPORTANT: PLEASE PROTECT THIS INFORMATION FOR FUTURE REFERENCE.</strong></span></p>
                                <p>Company Domain: (#CompanyPrimaryDomain#)</p>
                                <p>Control Panel: (#CompanyControlPanelUrl#)</p>
                                <p>Username: (#UserLogin#)</p>
                                <p>Password: To set your password please use following link</p>
                                <p><a name='_GoBack'></a> (#UserPasswordTempLink#)</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeUserDontSendPassword.ToString(),
                    Subject = "WELCOME – User Setup",
                    Template = @"
                                <p><span style='font-size: large;'><strong>WELCOME &ndash; User Setup</strong></span></p>
                                <p><br /> Welcome (#UserDisplayName#). You now have a user account at (#CompanyName#). Below you will find important details to assist you in accessing your account.</p>
                                <p>&nbsp;</p>
                                <p><span style='color: #ff0000;'><strong>IMPORTANT: PLEASE PROTECT THIS INFORMATION FOR FUTURE REFERENCE.</strong></span></p>
                                <p>Company Domain: (#CompanyPrimaryDomain#)</p>
                                <p>Control Panel: (#CompanyControlPanelUrl#)</p>
                                <p>Username: (#UserLogin#)</p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeUserSendPlainPasswordViaEmail.ToString(),
                    Subject = "WELCOME – User Setup",
                    Template = @"
                                <p><span style='font-size: large;'><strong>WELCOME &ndash; User Setup</strong></span></p>
                                <p><br /> Welcome (#UserDisplayName#). You now have a user account at (#CompanyName#). Below you will find important details to assist you in accessing your account.</p>
                                <p>&nbsp;</p>
                                <p><span style='color: #ff0000;'><strong>IMPORTANT: PLEASE PROTECT THIS INFORMATION FOR FUTURE REFERENCE.</strong></span></p>
                                <p>Company Domain: (#CompanyPrimaryDomain#)</p>
                                <p>Control Panel: (#CompanyControlPanelUrl#)</p>
                                <p>Username: (#UserLogin#)</p>
                                <p>Password: (#UserPassword#)</p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.ForgotPassword.ToString(),
                    Subject = "Password Reset Request",
                    Template = @"
                                <p><span style='font-size: large;'><strong>FORGOT PASSWORD</strong></span></p>
                                <p><br /> (#UserDisplayName#), OH NO! You forgot your password for your account at (#CompanyName#)! Don&rsquo;t worry. We got you covered.</p>
                                <p>&nbsp;</p>
                                <p>Control Panel: (#CompanyControlPanelUrl#)</p>
                                <p>Username: (#UserLogin#)</p>
                                <p>Password: To set your password please use following link</p>
                                <p><a name='_GoBack'></a> (#UserPasswordTempLink#)</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },

                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeCompanyCustomer.ToString(),
                    Subject = "WELCOME",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Customer, we&rsquo;re committed to ensuring a successful and exceptional experience for you. Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are your account details:</span></p>
                                <p><span style='font-size: small;'>1. Your Primary Domain is: (#CompanyPrimaryDomain#)</span></p>
                                <p><span style='font-size: small;'>2. Control Panel URL:&nbsp;(#ParentCompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>3. Username: (#UserLogin#)</span></p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)</span><span style='color: #000000;'>&nbsp;</span><span style='font-size: small;'>for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#). </span></p>
                                <p><span style='font-size: small;'>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog. </span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeCompanyReseller.ToString(),
                    Subject = "WELCOME",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Partner, we&rsquo;re committed to ensuring a successful and exceptional experience for you.&nbsp; Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are few details for your future reference:</span></p>
                                <p><span style='font-size: small;'>1. Control Panel URL:&nbsp;(#CompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>2. ARECORD IP (for your CP URL):&nbsp;(#ARecordIp#)</span></p>
                                <p><span style='font-size: small;'>3. Support URL:&nbsp;(#CompanySupportUrl#)</span></p>
                                <p><span style='font-size: small;'>4. Username:&nbsp;(#UserLogin#)</span></p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>First please bookmark your internal support site (#CompanySupportUrl#) to be accessed and utilized by your internal teams. You and your users can also access this content from within the left navigation of your control panel upon log-in.</span></p>
                                <p><span style='font-size: small;'>If you would like to enable your customer-facing support site please contact us at (#ParentCompanyContactPhone#) or (#ParentCompanySupportUrl#).</span></p>
                                <p><span style='font-size: small;'>For details regarding service enablement and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.ChangePassword.ToString(),
                    Subject = "Change Password Request",
                    Template = @"
                                <p><span style='font-size: large;'><strong>Your password changed</strong></span></p>
                                <p><br />The password for (#UserDisplayName#) account (#UserLogin#) was just changed.</p>
                                <p>&nbsp;</p>
                                <p>To set your password please use following link</p>
                                <p>(#UserPasswordTempLink#)</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.PasswordChanged.ToString(),
                    Subject = "Your Password Changed",
                    Template = @"
                                <p><span style='font-size: large;'><strong>Your password changed</strong></span></p>
                                <p><br />The password for (#UserDisplayName#) account (#UserLogin#) was just changed.</p>
                                <p>&nbsp;</p>
                                <p>Company administrator set up your new password.</p>
                                <p>New password will be provided to you by other form of communication..</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.PasswordChangedSendPlainPasswordViaEmail.ToString(),
                    Subject = "Your Password Changed",
                    Template = @"
                                <p><span style='font-size: large;'><strong>Your password changed</strong></span></p>
                                <p><br />The password for (#UserDisplayName#) account (#UserLogin#) was just changed. .</p>
                                <p>&nbsp;</p>
                                <p>Your new credentials are:</p>
                                <p>Username: (#UserLogin#)</p>
                                <p>Password: (#UserPassword#)</p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeCompanyCustomerPasswordViaEmail.ToString(),
                    Subject = "WELCOME",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Customer, we&rsquo;re committed to ensuring a successful and exceptional experience for you. Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are your account details:</span></p>
                                <p><span style='font-size: small;'>1. Your Primary Domain is: (#CompanyPrimaryDomain#)</span></p>
                                <p><span style='font-size: small;'>2. Control Panel URL:&nbsp;(#ParentCompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>3. Username: (#UserLogin#)</span></p>
                                <p><span style='font-size: small;'>4. Password: To set your password please use following link </span></p>
                                <p><a name='_GoBack'></a> (#UserPasswordTempLink#)</p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)</span><span style='color: #000000;'>&nbsp;</span><span style='font-size: small;'>for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#). </span></p>
                                <p><span style='font-size: small;'>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog. </span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeCompanyCustomerSendPlainPasswordViaEmail.ToString(),
                    Subject = "WELCOME",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Customer, we&rsquo;re committed to ensuring a successful and exceptional experience for you. Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are your account details:</span></p>
                                <p><span style='font-size: small;'>1. Your Primary Domain is: (#CompanyPrimaryDomain#)</span></p>
                                <p><span style='font-size: small;'>2. Control Panel URL:&nbsp;(#ParentCompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>3. Username: (#UserLogin#)</span></p>
                                <p><span style='font-size: small;'>4. Password: (#UserPassword#) </span></p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)</span><span style='color: #000000;'>&nbsp;</span><span style='font-size: small;'>for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#). </span></p>
                                <p><span style='font-size: small;'>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog. </span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.WelcomeCompanyResellerPasswordViaEmail.ToString(),
                    Subject = "WELCOME",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Partner, we&rsquo;re committed to ensuring a successful and exceptional experience for you.&nbsp; Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are few details for your future reference:</span></p>
                                <p><span style='font-size: small;'>1. Control Panel URL:&nbsp;(#CompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>2. ARECORD IP (for your CP URL):&nbsp;(#ARecordIp#)</span></p>
                                <p><span style='font-size: small;'>3. Support URL:&nbsp;(#CompanySupportUrl#)</span></p>
                                <p><span style='font-size: small;'>4. Username:&nbsp;(#UserLogin#)</span></p>
                                <p><span style='font-size: small;'>5. Password: To set your password please use following link </span></p>
                                <p><a name='_GoBack'></a> (#UserPasswordTempLink#)</p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>First please bookmark your internal support site (#CompanySupportUrl#) to be accessed and utilized by your internal teams. You and your users can also access this content from within the left navigation of your control panel upon log-in.</span></p>
                                <p><span style='font-size: small;'>If you would like to enable your customer-facing support site please contact us at (#ParentCompanyContactPhone#) or (#ParentCompanySupportUrl#).</span></p>
                                <p><span style='font-size: small;'>For details regarding service enablement and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
                },
                new EmailTemplate
            {
                Type = EmailTemplateType.WelcomeCompanyResellerSendPlainPasswordViaEmail.ToString(),
                Subject = "WELCOME",
                Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for choosing (#ParentCompanyName#) as your trusted technology partner. We&rsquo;re excited about the opportunity to assist (#CompanyName#) in growing your business through our robust service and solutions portfolio.</span></p>
                                <p><span style='font-size: small;'>As a new Partner, we&rsquo;re committed to ensuring a successful and exceptional experience for you.&nbsp; Streamlining technology and understanding the impact innovation can have on your organization will certainly differentiate you and is something we acknowledge and take very seriously.</span></p>
                                <p><span style='font-size: small;'>That being said, it&rsquo;s our hope that together we&rsquo;re not only capable of providing you with the tools to recruit and retain top talent and customers, but do so in a way that gives you the flexibility, security, and support you need and expect.</span></p>
                                <p><span style='font-size: small;'>Here are few details for your future reference:</span></p>
                                <p><span style='font-size: small;'>1. Control Panel URL:&nbsp;(#CompanyControlPanelUrl#)</span></p>
                                <p><span style='font-size: small;'>2. ARECORD IP (for your CP URL):&nbsp;(#ARecordIp#)</span></p>
                                <p><span style='font-size: small;'>3. Support URL:&nbsp;(#CompanySupportUrl#)</span></p>
                                <p><span style='font-size: small;'>4. Username:&nbsp;(#UserLogin#)</span></p>
                                <p><span style='font-size: small;'>5. Password: (#UserPassword#) </span></p>
                                <p>&nbsp;</p>
                                <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>First please bookmark your internal support site (#CompanySupportUrl#) to be accessed and utilized by your internal teams. You and your users can also access this content from within the left navigation of your control panel upon log-in.</span></p>
                                <p><span style='font-size: small;'>If you would like to enable your customer-facing support site please contact us at (#ParentCompanyContactPhone#) or (#ParentCompanySupportUrl#).</span></p>
                                <p><span style='font-size: small;'>For details regarding service enablement and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
            },
                new EmailTemplate
                {
                    Type = EmailTemplateType.Office365CustomerServiceEnabled.ToString(),
                    Subject = "Office 365 Service Enabled",
                    Template = @"
                                <div style='float: right; max-width: 200px; max-height: 200px;'>
                                <img src='(#ParentCompanyLogo#)' alt='Logo'  height='50'>
                                </div>
                                <p><span style='font-size: large;'><strong>DNS VALIDATION</strong></span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Dear (#CompanyName#),</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>Thank you for activating the Microsoft Office 365 service! In order to start using the service, you must validate your domain ownership. This is a simple process that requires a single DNS entry. Here is what is needed:</span></p>
                                <p><span style='font-size: small;'><strong>DNS ENTRY FOR (#Domain#)</strong></span></p>
                                <p><span style='font-size: small;'>You need to have access to your DNS records for your domain. This is typically through your Registrar (where you purchased your domain). If you are having trouble, please contact us at any time. We are here to help!</span></p>
                                <ol>
                                <li><span style='font-size: small;'>Log into your DNS Server for your DNS records</span></li>
                                <li><span style='font-size: small;'>Add the following DNS record:</span>
                                <ol>
                                <li>Record Type: TXT</li>
                                <li>Type: TXT</li>
                                <li>Value:&nbsp;(#TxtRecord#)</li>
                                <li>TTL: 3600</li>
                                </ol>
                                </li>
                                <li><span style='font-size: small;'>Open the control panel (#ParentCompanyControlPanelUrl#) and navigate to the Hosted Service Catalog.</span></li>
                                <li>In the Office 365 Service, click the Validate DNS button.</li>
                                <li>If the DNS validation is successfully confirmed, you can then start to add users. If the validation does not work, the above-listed TXT record may no longer be valid. Simply click the Resend DNS Validation Email button to generate a new TXT record and send it to your inbox. Immediately upon receiving the new record, repeat steps 1 through 4.</li>
                                </ol>
                                <p>&nbsp;</p>
                                <p><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></p>
                                <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                <p><span style='font-size: small;'>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)</span><span style='color: #000000;'>&nbsp;</span><span style='font-size: small;'>for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#). </span></p>
                                <p><span style='font-size: small;'>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog. </span></p>
                                <p><span style='font-size: small;'>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</span></p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: small;'>(#ParentCompanyName#)</span></p>
                                <p>&nbsp;</p>
                                <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                <p>&nbsp;</p>
                                <p>&nbsp;</p>
                                <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>
                                "
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.Office365PrimaryDomainVerifiedSetUp.ToString(),
                    Subject = "Office 365 Customer Service Set Up",
                    Template = @"
                                    <div style='float: right; max-width: 200px; max-height: 200px;'><img src='(#ParentCompanyLogo#)' alt='Logo' height='50' /></div>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                    <p>&nbsp;</p>
                                    <p>Thank you for selecting (#ParentCompanyName#) as your&nbsp;Partner of choice to begin taking advantage of the benefits Microsoft Office 365 can offer your organization. Your domain has been validated and you can complete the set-up process. Below you will find important details to assist you in the set-up as well as good things to remember. Let&rsquo;s get started!</p>
                                    <p>&nbsp;</p>
                                    <h4><strong>IMPORTANT LINKS TO BOOKMARK:</strong></h4>
                                    <p>Here are your account details:</p>
                                    <ul>
                                    <li>Control Panel for managing users / services: (#CompanyControlPanelUrl#)</li>
                                    <li>The Outlook Web Access (OWA) portal: https://outlook.office365.com</li>
                                    <li>Office 365 Advanced Management portal: https://outlook.office365.com</li>
                                    <li>Support documentation, FAQ's, etc.: (#CompanySupportUrl#)</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p><strong>ACCOUNT SET-UP &amp; MIGRATION OVERVIEW:</strong></p>
                                    <p>Here is a brief overview of the steps for setting up your new account. &nbsp;For a complete walk-through of the migration process, please view our helpful Migration articles at&nbsp;(#CompanySupportUrl#) . Search &gt; Migration Steps &gt; Microsoft Exchange Migration.</p>
                                    <ul>
                                    <li>Log into your Control Panel (#CompanyControlPanelUrl#) with admin privileges.</li>
                                    <li>Navigate to the Hosted Services Catalog and add the number of licenses desired.</li>
                                    <li>Go to the User List located in the left hand navigation, edit each user and assign the desired Office 365 license.</li>
                                    <li>Import your previous data into Exchange either by uploading the data via Outlook or using our Managed Migration service. &nbsp;You can review how to do this by visiting (#CompanySupportUrl#).</li>
                                    <li>If you activated the Hosted Microsoft Exchange within Office 365 and you are ready to direct all email to your new email server, change your MX, AutoDiscover and SPF Records to point to:
                                    <ul>
                                    <li>MX Records (remove all other MX records!)
                                    <ul>
                                    <li>Record Type: MX</li>
                                    <li>Host: @</li>
                                    <li>Value: (#CompanyPrimaryDomain#).mail.protection.outlook.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>autodiscover
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: autodiscover</li>
                                    <li>Value: autodiscover.outlook.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>Lync
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: lyncdiscover</li>
                                    <li>Value: webdir.online.lync.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>msoid
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: msoid</li>
                                    <li>Value: clientconfig.microsoftonline-p.net</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>sip
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: sip</li>
                                    <li>Value: sipdir.online.lync.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>MDM
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: enterpriseregistration</li>
                                    <li>Value: enterpriseregistration.windows.net</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>MDM
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: enterpriseenrollment</li>
                                    <li>Value: enterpriseenrollment-s.manage.microsoft.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SPF
                                    <ul>
                                    <li>Record Type: TXT</li>
                                    <li>Host: @</li>
                                    <li>Value: v=spf1 include:spf.protection.outlook.com -all</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SPF
                                    <ul>
                                    <li>Record Type: TXT</li>
                                    <li>Host: @</li>
                                    <li>Value: v=spf1 include:sharepointonline.com -all</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SRV<br />
                                    <ul>
                                    <li>Record Type: SRV</li>
                                    <li>Name: @</li>
                                    <li>Target: sipdir.online.lync.com</li>
                                    <li>Protocol: _tls</li>
                                    <li>Service: _sip</li>
                                    <li>Priority: 100</li>
                                    <li>Weight: 1</li>
                                    <li>Port 443</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SRV
                                    <ul>
                                    <li>Record Type: SRV</li>
                                    <li>Name: @</li>
                                    <li>Value: sipfed.online.lync.com</li>
                                    <li>Protocol: _tcp</li>
                                    <li>Service: _sipfederationtls</li>
                                    <li>Priority: 100</li>
                                    <li>Weight: 1</li>
                                    <li>Port 5061</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    </ul>
                                    </li>
                                    <li>Once that DNS change is complete, the email will flow to the new servers. &nbsp;Make sure all your email clients (and mobile clients) are pointing to the new server.</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                    <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                    <p>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)&nbsp;for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#).</p>
                                    <p>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</p>
                                    <p>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#)</p>
                                    <p>&nbsp;</p>
                                    <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>
                                "
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.Office365UserSetUp.ToString(),
                    Subject = "Office 365 User Service Set Up",
                    Template = @"
                                    <div style='float: right; max-width: 200px; max-height: 200px;'><img src='(#ParentCompanyLogo#)' alt='Logo' height='50' /></div>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                    <p>&nbsp;</p>
                                    <p>Welcome (#UserDisplayName#).&nbsp;Your Office 365 user account has been enabled and is ready to use. Below you will find important details to assist you in accessing your account.</p>
                                    <p>&nbsp;</p>
                                    <p><strong>IMPORTANT: PLEASE PROTECT THIS INFORMATION FOR FUTURE REFERENCE.</strong></p>
                                    <ul>
                                    <li>Primary Domain: (#CompanyPrimaryDomain#)</li>
                                    <li>Username:&nbsp;(#UserLogin#)</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p><strong>IMPORTANT LINKS TO BOOKMARK:</strong></p>
                                    <ul>
                                    <li>Control Panel for managing users / services: (#CompanyControlPanelUrl#)</li>
                                    <li>Office 365 portal for access Office applications, hosted email, and other subscribed services: <a href='https://login.microsoftonline.com'>https://login.microsoftonline.com</a></li>
                                    <li>Support documentation, FAQ's, etc.:&nbsp;(#CompanySupportUrl#)</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p>If you have a Business or Business Premium subscription, you can download your Office 365 desktop applications directly from the above Office 365 portal URL. For helpful video instructions on downloading and installing the Office 365 applications, please click the following link:</p>
                                    <p><a href='https://support.office.com/en-us/article/Video-Install-Office-on-your-PC-or-Mac-for-Office-365-for-business-b7071ece-237d-4f84-a67d-d8cf1d1f2e60?ui=en-US&amp;rs=en-US&amp;ad=US'>Video: How to install Office on your PC or Mac</a>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                    <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                    <p>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)&nbsp;for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#).</p>
                                    <p>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</p>
                                    <p>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#)</p>
                                    <p>&nbsp;</p>
                                    <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>
                                "
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.Office365AdditionalDomainVerified.ToString(),
                    Subject = "Office 365 additional domain added",
                    Template = @"
                                    <div style='float: right; max-width: 200px; max-height: 200px;'><img src='(#ParentCompanyLogo#)' alt='Logo' height='50' /></div>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>WELCOME</strong></span></span></p>
                                    <p>&nbsp;</p>
                                    <p>Thank you for selecting (#ParentCompanyName#) as your&nbsp;Partner of choice to begin taking advantage of the benefits Microsoft Office 365 can offer your organization. Your domain has been validated and you can complete the set-up process. Below you will find important details to assist you in the set-up as well as good things to remember. Let&rsquo;s get started!</p>
                                    <p>&nbsp;</p>
                                    <h4><strong>IMPORTANT LINKS TO BOOKMARK:</strong></h4>
                                    <p>Here are your account details:</p>
                                    <ul>
                                    <li>Control Panel for managing users / services: (#CompanyControlPanelUrl#)</li>
                                    <li>The Outlook Web Access (OWA) portal: https://outlook.office365.com</li>
                                    <li>Office 365 Advanced Management portal: https://outlook.office365.com</li>
                                    <li>Support documentation, FAQ's, etc.: (#CompanySupportUrl#)</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p><strong>ACCOUNT SET-UP &amp; MIGRATION OVERVIEW:</strong></p>
                                    <p>Here is a brief overview of the steps for setting up your new account. &nbsp;For a complete walk-through of the migration process, please view our helpful Migration articles at&nbsp;(#CompanySupportUrl#) . Search &gt; Migration Steps &gt; Microsoft Exchange Migration.</p>
                                    <ul>
                                    <li>Log into your Control Panel (#CompanyControlPanelUrl#) with admin privileges.</li>
                                    <li>Navigate to the Hosted Services Catalog and add the number of licenses desired.</li>
                                    <li>Go to the User List located in the left hand navigation, edit each user and assign the desired Office 365 license.</li>
                                    <li>Import your previous data into Exchange either by uploading the data via Outlook or using our Managed Migration service. &nbsp;You can review how to do this by visiting (#CompanySupportUrl#).</li>
                                    <li>If you activated the Hosted Microsoft Exchange within Office 365 and you are ready to direct all email to your new email server, change your MX, AutoDiscover and SPF Records to point to:
                                    <ul>
                                    <li>MX Records (remove all other MX records!)
                                    <ul>
                                    <li>Record Type: MX</li>
                                    <li>Host: @</li>
                                    <li>Value: (#CompanyDomain#).mail.protection.outlook.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>autodiscover
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: autodiscover</li>
                                    <li>Value: autodiscover.outlook.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>Lync
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: lyncdiscover</li>
                                    <li>Value: webdir.online.lync.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>msoid
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: msoid</li>
                                    <li>Value: clientconfig.microsoftonline-p.net</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>sip
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: sip</li>
                                    <li>Value: sipdir.online.lync.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>MDM
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: enterpriseregistration</li>
                                    <li>Value: enterpriseregistration.windows.net</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>MDM
                                    <ul>
                                    <li>Record Type: CNAME</li>
                                    <li>Host: enterpriseenrollment</li>
                                    <li>Value: enterpriseenrollment-s.manage.microsoft.com</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SPF
                                    <ul>
                                    <li>Record Type: TXT</li>
                                    <li>Host: @</li>
                                    <li>Value: v=spf1 include:spf.protection.outlook.com -all</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SPF
                                    <ul>
                                    <li>Record Type: TXT</li>
                                    <li>Host: @</li>
                                    <li>Value: v=spf1 include:sharepointonline.com -all</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SRV<br />
                                    <ul>
                                    <li>Record Type: SRV</li>
                                    <li>Name: @</li>
                                    <li>Target: sipdir.online.lync.com</li>
                                    <li>Protocol: _tls</li>
                                    <li>Service: _sip</li>
                                    <li>Priority: 100</li>
                                    <li>Weight: 1</li>
                                    <li>Port 443</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    <li>SRV
                                    <ul>
                                    <li>Record Type: SRV</li>
                                    <li>Name: @</li>
                                    <li>Value: sipfed.online.lync.com</li>
                                    <li>Protocol: _tcp</li>
                                    <li>Service: _sipfederationtls</li>
                                    <li>Priority: 100</li>
                                    <li>Weight: 1</li>
                                    <li>Port 5061</li>
                                    <li>TTL: 3600</li>
                                    </ul>
                                    </li>
                                    </ul>
                                    </li>
                                    <li>Once that DNS change is complete, the email will flow to the new servers. &nbsp;Make sure all your email clients (and mobile clients) are pointing to the new server.</li>
                                    </ul>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                                    <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                                    <p>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)&nbsp;for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#).</p>
                                    <p>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</p>
                                    <p>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#)</p>
                                    <p>&nbsp;</p>
                                    <p>&ldquo;<span style='font-size: small;'><em>Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)</em></span>&nbsp;<span style='font-size: small;'><em>was an easy one.&rdquo; &ndash; Customer Statement.</em></span></p>
                                    <p>&nbsp;</p>
                                    <p>&nbsp;</p>
                                    <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>
                                "
                },
                new EmailTemplate
                {
                    Type = EmailTemplateType.CustomSecureControlPanelActivation.ToString(),
                   Subject = "Custom Secure Control Panel URL: (#CustomSecureControlPanelURL#) - Account ID (#ControlPanelCompanyId#)",
                    Template = @"
                                <p>Hi,</p>
                                <p>&nbsp;</p>
                                <p><span style='font-size: large;'><strong>The (#CompanyName#) has Activated the Custom Secure Control Panel</strong></span></p>
                                <p>&nbsp;</p>
                                <p>Custom Secure Control Panel activation information.</p>
                                <p>&nbsp;</p>
                                <p>Company Name : (#CompanyName#)</p>
                                <p>Contact Name : (#ControlPanelContactName#)</p>
                                <p>Company Control Panel Url : (#CustomSecureControlPanelURL#)</p>
                                <p>Contact Phone : (#ControlPanelContactPhone#)</p>
                                <p>Company Address Street : (#ControlPanelAddressStreet#)</p>
                                <p>Company Address City : (#ControlPanelAddressCity#)</p>
                                <p>Company Address State : (#ControlPanelAddressState#)</p>
                                <p>Company Address Zip : (#ControlPanelAddressZip#)</p>
                                <p>&nbsp;</p>
                                <p>Please contact our Support Team at any time with questions and/or assistance.&nbsp; We are here to help!</p>
                                <p>&nbsp;</p>
                                <p>Best Regards,</p>
                                <p>(#CompanyName#) Support Team</p>
                                <p>(#CompanySupportUrl#)</p>
                                <p>(#CompanyContactPhone#)</p>"

                }

            };

            _dbContext.EmailTemplates.AddRange(emailTemplates);
            _dbContext.SaveChanges();

            return this;
        }

        public EmailTemplatesUtilities SeedTransitionReportEmailTemplate()
        {
            var emailTemplate = new EmailTemplate
            {
                Type = EmailTemplateType.Office365TransitionReport.ToString(),
                Subject = "Office 365 Transition Report",
                Template = @"
                            <div style='float: right; max-width: 200px; max-height: 200px;'><img src='(#ParentCompanyLogo#)' alt='Logo' height='50' /></div>
                            <p><span style='color: #000000;'><span style='font-size: large;'><strong>Office 365 Transition Report</strong></span></span></p>
                            <p>&nbsp;</p>
                            <p>Dear (#CompanyName#),</p>
                            <p>&nbsp;</p>
                            <p>Office 365 transition process has finished with folowing results:</p>
                            <ul>
                            <li>(#TransitionDeletedUsersCount#) user/s deleted from Office 365 platform</li>
                            <li>(#TransitionAdminUsersCount#) user/s promoted to Office 365 Admin role</li>
                            <li>(#TransitionLicensesUsersCount#) users/s with Office 365 licenses assigned/removed/changed</li>
                            </ul>
                            <p>&nbsp;</p>
                            <p>Users choosen to be left as they are for latter discusion:</p>
                            <p>(#TransitionDiscussLicensesUsers#)</p>
                            <p>&nbsp;</p>
                            <p>Users which didn't finish process successfully:</p>
                            <p>&nbsp;(#TransitionFailedUsers#)</p>
                            <p>&nbsp;</p>
                            <p>&nbsp;</p>
                            <p><span style='color: #000000;'><span style='font-size: large;'><strong>YOU HAD ME AT HELLO</strong></span></span></p>
                            <p><span style='font-size: small;'><strong>SUPPORT DETAILS </strong></span></p>
                            <p>For support, please bookmark our support site&nbsp;(#ParentCompanySupportUrl#)&nbsp;for instant accessibility. If support extends beyond the content available on the site please follow the prompts or reach out to (#ParentCompanyContactPhone#).</p>
                            <p>For details regarding services rendered and getting started, please reference our Service Set-up Letters in your Control Panel within your Hosted Services catalog.</p>
                            <p>Again, thank you for entrusting (#ParentCompanyName#) for your technology needs. We look forward to working with you and your teams. Please don&rsquo;t hesitate to reach out at any time.</p>
                            <p>&nbsp;</p>
                            <p>(#ParentCompanyName#)</p>
                            <p>&nbsp;</p>
                            <p><em>&ldquo;Simplicity and genuine partnership, hand-in-hand with the very services I need to advance my business forward is the very reason the decision to migrate to the Cloud with&nbsp;(#ParentCompanyName#)&nbsp;was an easy one.&rdquo; &ndash; Customer Statement.</em></p>
                            <p>&nbsp;</p>
                            <p>&nbsp;</p>
                            <p>(#ParentCompanyName#) | (#ParentCompanyWebSite#) | (#ParentCompanyContactPhone#) | (#ParentCompanySupportUrl#)</p>"
            };

            _dbContext.EmailTemplates.Add(emailTemplate);
            _dbContext.SaveChanges();

            return this;
        }

    }
}
