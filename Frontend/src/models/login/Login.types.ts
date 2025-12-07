export interface LoginResponse {
    accessToken: string;
    tokenType: string;

    requires2FA?: boolean;      // user must verify 2FA
    requires2FASetup?: boolean; // user must set up 2FA

    userId?: string;
    message?: string;
}