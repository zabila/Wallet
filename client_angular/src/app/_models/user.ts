export interface User {
    id: string;
    email?: string;
    userName?: string;
    firstName?: string;
    lastName?: string;
    phoneNumber?: string;
    telegramUserName?: string;
    telegramUserId: number;
    isEmailConfirmed: boolean;
    isPhoneNumberConfirmed: boolean;
    roles?: string[];
}
