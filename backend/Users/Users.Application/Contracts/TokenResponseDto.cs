namespace Users.Application.Contracts
{
    public sealed class TokenResponseDto
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
