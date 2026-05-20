import type { Coach } from "../types";
import { client } from "./client";

export async function getCoaches(page = 1, pageSize = 50): Promise<Coach[]> {
    const { data } = await client.get<Coach[]>('/Coach', { params: { page, pageSize } });
    return data;
}

export async function registerCoach(firstName: string, email: string, password: string): Promise<Coach> {
    const { data } = await client.post<Coach>('/Coach/register', { firstName, email, password });
    return data;
}

export async function deleteCoach(id: string): Promise<void> {
    await client.delete(`/Coach/${id}`);
}