namespace CloudPlus.QueueModels.Users.Commands
{
	public interface IDeleteUserCommand
	{
		int UserId { get; set; }
		int CompanyId { get; set; }
		int UserLoggedIn { get; set; }
	}
}