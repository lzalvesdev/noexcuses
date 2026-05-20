export interface User {
    name: string;
    email: string;
    roles: string[];
}

export interface Coach {
    id: string;
    firstName: string;
    email: string;
    specialities: Speciality[];
    athletes: AthleteDetail[];
}

export interface Speciality {
    id: number;
    description: string;
}

export interface Athlete {
    id: string;
    firstName: string;
    email: string;
    coachId: string;
}

export interface AthleteDetail {
    id: string;
    firstName: string;
    email: string;
}

export const UserRole = {
    Admin: 'Admin',
    Manager: 'Manager',
    Coach: 'Coach',
    Athlete: 'Athlete',
    User: 'User',
} as const;

export type UserRole = typeof UserRole[keyof typeof UserRole];