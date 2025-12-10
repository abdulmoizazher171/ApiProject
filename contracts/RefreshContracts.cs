namespace MyApiProject.contracts
{
    /// <summary>
    /// DTO used for transmitting the refresh token data back to the client 
    /// (e.g., when a user successfully logs in or refreshes their access token).
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// The raw token string the client must store and use to request a new access token.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// The date and time the refresh token expires.
        /// </summary>
        public DateTime Expires { get; set; }
        
        // Note: Fields like TokenID, UserID, and Revoked are intentionally omitted 
        // as they are internal administrative details and should not be exposed to the client.
    }
}