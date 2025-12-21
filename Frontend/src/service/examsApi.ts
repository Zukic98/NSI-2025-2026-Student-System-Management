export interface Exam {
  id: string;
  courseName: string;
  dateTime: string;
  location: string;
}

export let MOCK_EXAMS: Exam[] = [
  {
    id: 'e1',
    courseName: 'Algorithms and Data Structures',
    dateTime: '2025-12-28 10:00',
    location: 'Room 101',
  },
  {
    id: 'e2',
    courseName: 'Signals and Systems',
    dateTime: '2026-01-05 12:30',
    location: 'Room 202',
  },
];

export const fetchExams = async (): Promise<Exam[]> => {
  // eslint-disable-next-line no-console
  console.log(
    'examsApi.fetchExams: returning MOCK_EXAMS (count=',
    MOCK_EXAMS.length,
    ')',
  );

  return new Promise((resolve) =>
    setTimeout(() => resolve(MOCK_EXAMS), 500),
  );
};

/* =========================
   CREATE EXAM (TASK 444)
   ========================= */

export interface CreateExamPayload {
  courseName: string;
  dateTime: string;
  location: string;
}

export const createExam = async (
  payload: CreateExamPayload,
): Promise<void> => {
  // eslint-disable-next-line no-console
  console.log('examsApi.createExam MOCK:', payload);

  const newExam: Exam = {
    id: `e${Date.now()}`, // simple unique id
    courseName: payload.courseName,
    dateTime: payload.dateTime,
    location: payload.location,
  };

  // Simulate database insert
  MOCK_EXAMS = [...MOCK_EXAMS, newExam];

  // Simulate backend latency
  return new Promise((resolve) => setTimeout(resolve, 400));
};

export const deleteExam = async (id: string): Promise<void> => {
  // eslint-disable-next-line no-console
  console.log('examsApi.deleteExam MOCK:', id);

  MOCK_EXAMS = MOCK_EXAMS.filter((e) => e.id !== id);

  // Simulate backend latency
  return new Promise((resolve) => setTimeout(resolve, 300));
};
