import type { Athlete } from "../types";
import { client } from "./client";

export async function getAthletes(page = 1, pageSize = 50): Promise<Athlete[]> {
    const { data } = await client.get<Athlete[]>('/Athlete', { params: { page, pageSize } });
    return data;
}

export async function registerAthlete(firstName: string, email: string, password: string, coachId: string): Promise<Athlete> {
    const { data } = await client.post<Athlete>('/Athlete/register', { firstName, email, password, coachId });
    return data;
}

export async function updateAthleteCoach(id: string, coachId: string): Promise<void> {
    await client.put(`/Athlete/${id}`, { coachId });
}

export async function deleteAthlete(id: string): Promise<void> {
    await client.delete(`/Athlete/${id}`);
}