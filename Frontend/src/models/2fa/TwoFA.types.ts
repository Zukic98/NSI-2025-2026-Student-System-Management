export type TwoFASetupResponse = {
    qrCodeBase64: string;
    manualEntryKey: string;
    otpAuthUri: string;
    message?: string;
};

export type TwoFAConfirmResponse = {
    success: boolean;
    message?: string;
    recoveryCodes?: string[];
};

export type TwoFALoginResponse = {
  accessToken: string;
  tokenType: string;
  expiresOn: number;

  email: string;
  userId: string;
  role: string;
  tenantId: string;
  fullName: string;
};