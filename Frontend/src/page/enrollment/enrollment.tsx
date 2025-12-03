"use client"

import { useState } from "react"
import {
  CContainer,
  CRow,
  CCol,
  CCard,
  CCardBody,
  CButton,
  CModal,
  CModalHeader,
  CModalTitle,
  CModalBody,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CFormSelect,
  CBadge,
  CAlert,
} from "@coreui/react"
import CIcon from "@coreui/icons-react"
import { cilSearch, cilCheckCircle } from "@coreui/icons"
import "@coreui/coreui/dist/css/coreui.min.css"

interface Course {
  id: string
  name: string
  code: string
  professor: string
  ects: number
  status: "required" | "elective" | "enrolled"
}

export default function EnrollmentPage() {
  const [courses, setCourses] = useState<Course[]>([
    {
      id: "1",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "required",
    },
    {
      id: "2",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "enrolled",
    },
    {
      id: "3",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "elective",
    },
    {
      id: "4",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "required",
    },
    {
      id: "5",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "required",
    },
    {
      id: "6",
      name: "Algorithms and Data Structures",
      code: "ADSII",
      professor: "Prof. H S",
      ects: 6,
      status: "required",
    },
  ])

  const [searchQuery, setSearchQuery] = useState("")
  const [selectedSemester, setSelectedSemester] = useState("all")
  const [selectedCourse, setSelectedCourse] = useState<Course | null>(null)
  const [showModal, setShowModal] = useState(false)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)
  const [isEnrolling, setIsEnrolling] = useState(false)

  const handleEnrollClick = (course: Course) => {
    setSelectedCourse(course)
    setShowModal(true)
  }

  const handleConfirmEnrollment = async () => {
    if (!selectedCourse) return

    setIsEnrolling(true)

    try {
      const response = await fetch("/api/enrollment", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          courseId: selectedCourse.id,
          courseName: selectedCourse.name,
          courseCode: selectedCourse.code,
        }),
      })

      if (!response.ok) {
        throw new Error("Enrollment failed")
      }

      setCourses((prevCourses) =>
        prevCourses.map((course) =>
          course.id === selectedCourse.id ? { ...course, status: "enrolled" as const } : course,
        ),
      )

      setSuccessMessage(`You have successfully enrolled in "${selectedCourse.name}"!`)
      setShowModal(false)

      setTimeout(() => {
        setSuccessMessage(null)
      }, 5000)
    } catch (error) {
      console.error("Enrollment error:", error)
      alert("Failed to enroll. Please try again.")
    } finally {
      setIsEnrolling(false)
    }
  }

  const filteredCourses = courses.filter((course) => {
    const matchesSearch =
      course.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      course.code.toLowerCase().includes(searchQuery.toLowerCase())
    const matchesSemester = selectedSemester === "all" || course.status === selectedSemester
    return matchesSearch && matchesSemester
  })

  return (
    <div style={{ backgroundColor: "#c5d5e4", minHeight: "100vh", padding: "40px 60px" }}>
      <CContainer fluid>
        <div className="mb-5">
          <div className="d-flex justify-content-between align-items-start mb-4">
            <h1
              className="fw-bold m-0"
              style={{
                color: "#1e4d8b",
                fontSize: "48px",
                flex: 1,
                textAlign: "center",
              }}
            >
              Course Enrollment
            </h1>
            {successMessage && (
              <CAlert
                color="light"
                className="d-flex align-items-center gap-2 mb-0"
                style={{
                  backgroundColor: "#d4edda",
                  border: "1px solid #c3e6cb",
                  borderRadius: "4px",
                  padding: "12px 20px",
                  fontSize: "14px",
                  position: "absolute",
                  right: "60px",
                  top: "40px",
                  minWidth: "400px",
                }}
              >
                <CIcon icon={cilCheckCircle} style={{ color: "#155724" }} />
                <span style={{ color: "#155724" }}>{successMessage}</span>
              </CAlert>
            )}
          </div>

          <CRow className="g-3 mb-5">
            <CCol md={8}>
              <CInputGroup style={{ backgroundColor: "white", borderRadius: "4px" }}>
                <CInputGroupText style={{ backgroundColor: "white", border: "none" }}>
                  <CIcon icon={cilSearch} />
                </CInputGroupText>
                <CFormInput
                  placeholder="Search for a course"
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  style={{ border: "none", fontSize: "16px" }}
                />
              </CInputGroup>
            </CCol>
            <CCol md={4}>
              <CFormSelect
                value={selectedSemester}
                onChange={(e) => setSelectedSemester(e.target.value)}
                style={{
                  backgroundColor: "white",
                  fontSize: "16px",
                }}
              >
                <option value="all">Semester: All</option>
                <option value="required">Required</option>
                <option value="elective">Elective</option>
                <option value="enrolled">Enrolled</option>
              </CFormSelect>
            </CCol>
          </CRow>
        </div>

        <CRow className="g-4">
          {filteredCourses.map((course) => (
            <CCol key={course.id} lg={4} md={6}>
              <CCard
                style={{
                  backgroundColor: course.status === "required" ? "white" : "#f8f9fa",
                  border: course.status === "required" ? "3px solid #1e4d8b" : "1px solid #dee2e6",
                  borderRadius: "16px",
                  boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
                  minHeight: "280px",
                }}
              >
                <CCardBody className="d-flex flex-column p-4">
                  <h5 className="fw-bold mb-1" style={{ fontSize: "20px", color: "#000" }}>
                    {course.name}
                  </h5>
                  <p className="text-muted mb-3" style={{ fontSize: "16px" }}>
                    {course.code}
                  </p>
                  <p className="mb-4" style={{ fontSize: "16px", color: "#000" }}>
                    {course.professor}
                  </p>

                  <div className="d-flex align-items-center justify-content-between mb-4 mt-auto">
                    <span className="fw-semibold" style={{ fontSize: "16px", color: "#000" }}>
                      {course.ects} ECTS
                    </span>
                    <CBadge
                      style={{
                        backgroundColor:
                          course.status === "required"
                            ? "#e8b4b8"
                            : course.status === "elective"
                              ? "#90c695"
                              : "#9ca3af",
                        color:
                          course.status === "required"
                            ? "#721c24"
                            : course.status === "elective"
                              ? "#155724"
                              : "#495057",
                        padding: "6px 16px",
                        borderRadius: "4px",
                        fontSize: "14px",
                        fontWeight: "500",
                      }}
                    >
                      {course.status === "required"
                        ? "Required"
                        : course.status === "elective"
                          ? "Elective"
                          : "Enrolled"}
                    </CBadge>
                  </div>

                  <CButton
                    style={{
                      backgroundColor: course.status === "enrolled" ? "#9ca3af" : "#1e4d8b",
                      borderColor: course.status === "enrolled" ? "#9ca3af" : "#1e4d8b",
                      borderRadius: "8px",
                      fontWeight: "600",
                      fontSize: "16px",
                      padding: "10px",
                      color: "white",
                    }}
                    className="w-100"
                    onClick={() => handleEnrollClick(course)}
                    disabled={course.status === "enrolled"}
                  >
                    {course.status === "enrolled" ? "Enrolled" : "Enroll"}
                  </CButton>
                </CCardBody>
              </CCard>
            </CCol>
          ))}
        </CRow>
      </CContainer>

      <CModal visible={showModal} onClose={() => setShowModal(false)} alignment="center" size="lg">
        <CModalHeader style={{ border: "none", paddingBottom: 0 }}>
          <CModalTitle style={{ fontSize: "28px", fontWeight: "bold" }}>Course Details</CModalTitle>
        </CModalHeader>
        <CModalBody style={{ padding: "60px 40px" }}>
          {selectedCourse && (
            <div style={{ textAlign: "center" }}>
              <h2 style={{ fontSize: "42px", fontWeight: "bold", marginBottom: "20px" }}>{selectedCourse.name}</h2>
              <p style={{ fontSize: "32px", color: "#6c757d", marginBottom: "40px" }}>{selectedCourse.code}</p>
              <p style={{ fontSize: "32px", color: "#000" }}>
                <span style={{ fontWeight: "bold" }}>Professor:</span> {selectedCourse.professor.replace("Prof. ", "")}
              </p>
            </div>
          )}
        </CModalBody>
        <div style={{ textAlign: "center", paddingBottom: "60px" }}>
          <CButton
            style={{
              backgroundColor: "#1e4d8b",
              borderColor: "#1e4d8b",
              borderRadius: "50px",
              padding: "18px 80px",
              fontSize: "24px",
              fontWeight: "600",
              color: "white",
            }}
            onClick={handleConfirmEnrollment}
            disabled={isEnrolling}
          >
            {isEnrolling ? "Enrolling..." : "Confirm enrollment"}
          </CButton>
        </div>
      </CModal>
    </div>
  )
}
