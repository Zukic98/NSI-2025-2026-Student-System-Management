interface CreateAssignmentDTO {
  course: string
  name: string
  description: string
  dueDate: Date
  maxPoints: number
}

interface Assignment {
  id: string
  course: string
  name: string
  faculty: string
  maxPoints: number
  major: string
  description: string
  dueDate: Date
}

// Mock data store
let mockAssignments: Assignment[] = [
  {
    id: "35009",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Introduction to computer vision",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35012",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Advanced computer vision techniques",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35011",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Computer vision applications",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35010",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Machine learning for vision",
    dueDate: new Date("2024-12-31"),
  },
    {
    id: "35011",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Computer vision applications",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35010",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Machine learning for vision",
    dueDate: new Date("2024-12-31"),
  },
    {
    id: "35011",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Computer vision applications",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35010",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Machine learning for vision",
    dueDate: new Date("2024-12-31"),
  },
    {
    id: "35011",
    course: "NSI",
    name: "Computer someth",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Computer vision applications",
    dueDate: new Date("2024-12-31"),
  },
  {
    id: "35010",
    course: "NSI",
    name: "Computer Vision",
    faculty: "ETF",
    maxPoints: 3,
    major: "Computer Science",
    description: "Machine learning for vision",
    dueDate: new Date("2024-12-31"),
  },
]

// Simulate API delay
const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms))

const mockAPI = {
  getAssignments: async (): Promise<Assignment[]> => {
    await delay(300)
    return [...mockAssignments]
  },

  createAssignment: async (dto: CreateAssignmentDTO): Promise<Assignment> => {
    await delay(500)
    const newAssignment: Assignment = {
      id: String(Math.floor(Math.random() * 90000) + 10000),
      course: dto.course,
      name: dto.name,
      faculty: "ETF",
      maxPoints: dto.maxPoints,
      major: "Computer Science",
      description: dto.description,
      dueDate: dto.dueDate,
    }
    mockAssignments.push(newAssignment)
    return newAssignment
  },

  updateAssignment: async (id: string, data: Partial<Assignment>): Promise<Assignment> => {
    await delay(500)
    const index = mockAssignments.findIndex((a) => a.id === id)
    if (index === -1) {
      throw new Error("Assignment not found")
    }
    mockAssignments[index] = { ...mockAssignments[index], ...data }
    return mockAssignments[index]
  },

  deleteAssignment: async (id: string): Promise<void> => {
    await delay(500)
    mockAssignments = mockAssignments.filter((a) => a.id !== id)
  },
}

export default mockAPI
