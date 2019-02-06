param($AdminUsername,$AdminPassword,$DistributionGroupName,$RemovalMemberSMTPAddress)

#####################################################################################################################################################################
function Remove-O365GroupMember()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
    
    # Import Exchange Online PowerShell Module
    $Session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid/ -Credential $O365Cred -Authentication Basic -AllowRedirection
	Import-PSSession $Session -DisableNameChecking
		 
	# Remove the DistributionGroup member
	Remove-DistributionGroupMember -Identity $DistributionGroupName -Member $RemovalMemberSMTPAddress -confirm $false
		 
	# Remove the PSSession
	Remove-PSSession $Session
         
}

# Import the Microsoft Online module
Import-Module MSOnline

Remove-O365GroupMember 