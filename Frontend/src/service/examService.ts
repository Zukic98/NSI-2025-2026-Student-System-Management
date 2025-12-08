import type { ExamDTO } from "../models/exams/Exam.types";
import type { ExamRegistrationDTO } from "../models/exams/ExamRegistration.types";

// TODO: replace these URLs with actual backend routes when Exam CRUD API is built.
/* const API = {
    eligible: "/api/exams/eligible",
    registered: "/api/exams/registered",
    register: (id: number) => `/api/exams/${id}/register`,
};
 */
export const examService = {
    async getEligibleExams(): Promise<ExamDTO[]> {
        // TEMP: until backend API exists
        return [
            {
                id: 1,
                courseId: "GUID-HERE",
                courseName: "Algorithms and Data Structures",
                courseCode: "ADS101",
                examDate: "2025-02-10T09:00:00Z",
                regDeadline: "2025-02-05T23:59:00Z",
                location: "Room 301 - ETF",
            }
        ];
    },

    async getRegisteredExams(): Promise<ExamRegistrationDTO[]> {
        // TEMP: simulate backend
        return [];
    },

    /* async registerForExam(examId: number): Promise<void> {
        // TEMP: no backend yet
        return;
    } */
};
