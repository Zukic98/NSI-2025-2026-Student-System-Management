"use client";

import { useEffect, useState } from "react";
import {
  CCard,
  CCardBody,
  CCardText,
  CCardTitle,
  CCol,
  CRow,
  CSpinner,
  CAlert,
  CButton,
  CFormInput,
  CInputGroup,
  CInputGroupText,
  CListGroup,
  CListGroupItem,
} from "@coreui/react";
import "@coreui/coreui/dist/css/coreui.min.css";
import {
  Search,
  ArrowRight,
  CalendarIcon,
  FileText,
  Award,
  MoreHorizontal,
} from "lucide-react";
import {
  format,
  startOfMonth,
  endOfMonth,
  eachDayOfInterval,
  isToday,
} from "date-fns";
import { getDashboardData } from "../../service/dashboard/api";

interface DashboardData {
  user: {
    name: string;
    faculty: string;
  };
  stats: {
    gpa: number;
    enrolledCourses: number;
    attendanceRate: number;
    deadlines: {
      count: number;
      period: string;
    };
  };
  courses: Array<{
    id: number;
    title: string;
    professor: string;
  }>;
  upcomingTasks: Array<{
    id: number;
    course: string;
    task: string;
    day: string;
  }>;
  calendar: {
    currentMonth: string;
    highlightedDates: number[];
  };
  quickActions: Array<{
    id: number;
    label: string;
    icon: string;
    color: string;
  }>;
}

export default function DashboardPage() {
  const [data, setData] = useState<DashboardData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentDate] = useState(new Date(2025, 10, 1)); // November 2025

  useEffect(() => {
    async function fetchDashboardData() {
      try {
        setLoading(true);
        const result = await getDashboardData();
        setData(result);
      } catch (err) {
        setError(err instanceof Error ? err.message : "An error occurred");
      } finally {
        setLoading(false);
      }
    }

    fetchDashboardData();
  }, []);

  // Generate calendar days
  const monthStart = startOfMonth(currentDate);
  const monthEnd = endOfMonth(currentDate);
  const daysInMonth = eachDayOfInterval({ start: monthStart, end: monthEnd });

  // Get day of week for first day (0 = Sunday, 1 = Monday, etc.)
  const firstDayOfWeek = monthStart.getDay();

  // Generate array to include empty cells for days before month starts
  const calendarDays = Array(firstDayOfWeek).fill(null).concat(daysInMonth);

  const getDateColor = (date: Date | null) => {
    if (!date || !data) return "";
    const day = date.getDate();

    if (data.calendar.highlightedDates.includes(day)) {
      // Cycle through colors for different dates
      const colors = ["#a78bfa", "#60a5fa", "#34d399"]; // purple, blue, green
      const index = data.calendar.highlightedDates.indexOf(day) % colors.length;
      return colors[index];
    }
    return "";
  };

  if (loading) {
    return (
      <div
        className="d-flex justify-content-center align-items-center"
        style={{ minHeight: "400px" }}
      >
        <CSpinner color="primary" />
      </div>
    );
  }

  if (error) {
    return (
      <CAlert color="danger">
        <h4 className="alert-heading">Error loading dashboard</h4>
        <p>{error}</p>
      </CAlert>
    );
  }

  if (!data) {
    return null;
  }

  return (
    <div
      style={{
        padding: "2rem",
        backgroundColor: "#d1dae6",
        minHeight: "100vh",
      }}
    >
      {/* Header */}
      <CRow className="mb-4">
        <CCol xs={12} lg={8}>
          <h1
            className="mb-3"
            style={{ fontSize: "2.5rem", fontWeight: 600, color: "#1e3a5f" }}
          >
            Welcome back, {data.user.name}!
          </h1>
        </CCol>
        <CCol xs={12} lg={4}>
          <CInputGroup>
            <CFormInput
              placeholder="Search courses, grades ..."
              style={{ backgroundColor: "white" }}
            />
            <CInputGroupText
              style={{ backgroundColor: "white", border: "none" }}
            >
              <Search size={20} />
            </CInputGroupText>
          </CInputGroup>
        </CCol>
      </CRow>

      {/* Stats Cards */}
      <CRow className="mb-4 g-3">
        <CCol xs={6} md={3}>
          <CCard
            style={{ border: "none", boxShadow: "0 2px 4px rgba(0,0,0,0.1)" }}
          >
            <CCardBody className="text-center">
              <CCardTitle
                style={{
                  fontSize: "clamp(1.2rem, 3vw, 2.5rem)",
                  whiteSpace: "nowrap",
                  fontWeight: "bold",
                  marginBottom: "0.5rem",
                }}
              >
                {data.stats.gpa}
              </CCardTitle>
              <CCardText style={{ fontSize: "0.875rem", color: "#6b7280" }}>
                GAP
              </CCardText>
            </CCardBody>
          </CCard>
        </CCol>
        <CCol xs={6} md={3}>
          <CCard
            style={{ border: "none", boxShadow: "0 2px 4px rgba(0,0,0,0.1)" }}
          >
            <CCardBody className="text-center">
              <CCardTitle
                style={{
                  fontSize: "clamp(1.2rem, 3vw, 2.5rem)",
                  whiteSpace: "nowrap",
                  fontWeight: "bold",
                  marginBottom: "0.5rem",
                }}
              >
                {data.stats.enrolledCourses}
              </CCardTitle>
              <CCardText style={{ fontSize: "0.875rem", color: "#6b7280" }}>
                Enrolled courses
              </CCardText>
            </CCardBody>
          </CCard>
        </CCol>
        <CCol xs={6} md={3}>
          <CCard
            style={{ border: "none", boxShadow: "0 2px 4px rgba(0,0,0,0.1)" }}
          >
            <CCardBody className="text-center">
              <CCardTitle
                style={{
                  fontSize: "clamp(1.2rem, 3vw, 2.5rem)",
                  whiteSpace: "nowrap",
                  fontWeight: "bold",
                  marginBottom: "0.5rem",
                }}
              >
                {data.stats.attendanceRate}%
              </CCardTitle>
              <CCardText style={{ fontSize: "0.875rem", color: "#6b7280" }}>
                Attendance rate
              </CCardText>
            </CCardBody>
          </CCard>
        </CCol>
        <CCol xs={6} md={3}>
          <CCard
            style={{ border: "none", boxShadow: "0 2px 4px rgba(0,0,0,0.1)" }}
          >
            <CCardBody className="text-center">
              <CCardTitle
                style={{
                  fontSize: "clamp(1.2rem, 3vw, 2.5rem)",
                  whiteSpace: "nowrap",
                  fontWeight: "bold",
                  marginBottom: "0.5rem",
                }}
              >
                {data.stats.deadlines.count} deadlines
              </CCardTitle>
              <CCardText style={{ fontSize: "0.875rem", color: "#6b7280" }}>
                {data.stats.deadlines.period}
              </CCardText>
            </CCardBody>
          </CCard>
        </CCol>
      </CRow>

      <CRow className="g-3">
        <CCol xs={12} lg={8}>
          {/* My Courses */}
          <h2
            className="mb-3"
            style={{ fontSize: "1.5rem", fontWeight: 600, color: "#1e3a5f" }}
          >
            My courses
          </h2>
          <CRow className="mb-4 g-3">
            {data.courses.map((course) => (
              <CCol key={course.id} xs={12} md={6} lg={4}>
                <CCard
                  style={{
                    border: "none",
                    backgroundColor: "#f9f7f0",
                    boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
                    cursor: "pointer",
                    transition: "transform 0.2s",
                  }}
                  className="h-100"
                >
                  <CCardBody className="d-flex flex-column justify-content-between">
                    <div>
                      <div className="d-flex justify-content-end mb-2">
                        <ArrowRight size={20} />
                      </div>
                      <CCardTitle
                        style={{
                          fontSize: "1.125rem",
                          fontWeight: 600,
                          marginBottom: "1rem",
                        }}
                      >
                        {course.title}
                      </CCardTitle>
                    </div>
                    <CCardText
                      style={{ fontSize: "0.875rem", color: "#6b7280" }}
                    >
                      {course.professor}
                    </CCardText>
                  </CCardBody>
                </CCard>
              </CCol>
            ))}
          </CRow>

          {/* Upcoming this week */}
          <h2
            className="mb-3"
            style={{ fontSize: "1.5rem", fontWeight: 600, color: "#1e3a5f" }}
          >
            Upcoming this week
          </h2>
          <CListGroup>
            {data.upcomingTasks.map((task) => (
              <CListGroupItem
                key={task.id}
                style={{
                  backgroundColor: "transparent",
                  backdropFilter: "none",
                  border: "none",
                  marginBottom: "0.5rem",
                  borderRadius: "8px",
                  boxShadow: "none",
                }}
              >
                <div className="d-flex justify-content-between align-items-center">
                  <div className="d-flex align-items-start gap-3">
                    <CalendarIcon
                      size={20}
                      style={{ marginTop: "4px", color: "#6b7280" }}
                    />
                    <div>
                      <div style={{ fontWeight: 600 }}>
                        {task.course} - {task.task}
                      </div>
                      <div
                        style={{
                          fontSize: "0.875rem",
                          color: "#6b7280",
                          fontStyle: "italic",
                        }}
                      >
                        {task.day}
                      </div>
                    </div>
                  </div>
                  <CButton
                    color="primary"
                    size="sm"
                    style={{ backgroundColor: "#4f46e5", border: "none" }}
                  >
                    Submit
                  </CButton>
                </div>
              </CListGroupItem>
            ))}
          </CListGroup>
        </CCol>

        <CCol xs={12} lg={4}>
          <CCard
            style={{ border: "none", boxShadow: "0 2px 4px rgba(0,0,0,0.1)" }}
          >
            <CCardBody>
              {/* Calendar */}
              <div className="mb-4">
                <div className="d-flex justify-content-center mb-3">
                  <h3 style={{ fontSize: "1rem", fontWeight: 600 }}>
                    {format(currentDate, "MMMM yyyy")}
                  </h3>
                </div>

                {/* Calendar Grid */}
                <div
                  style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(7, 1fr)",
                    gap: "0.5rem",
                  }}
                >
                  {/* Weekday headers */}
                  {["S", "M", "T", "W", "T", "F", "S"].map((day, idx) => (
                    <div
                      key={idx}
                      style={{
                        textAlign: "center",
                        fontSize: "0.75rem",
                        fontWeight: 600,
                        color: "#9ca3af",
                        padding: "0.5rem 0",
                      }}
                    >
                      {day}
                    </div>
                  ))}

                  {/* Calendar days */}
                  {calendarDays.map((date, idx) => {
                    const bgColor = date ? getDateColor(date) : "";
                    const textColor = bgColor ? "white" : "#1f2937";
                    const isCurrentDay = date && isToday(date);

                    return (
                      <div
                        key={idx}
                        style={{
                          aspectRatio: "1",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                          fontSize: "0.875rem",
                          borderRadius: "8px",
                          backgroundColor:
                            bgColor || (date ? "transparent" : "transparent"),
                          color: date ? textColor : "transparent",
                          fontWeight: bgColor || isCurrentDay ? 600 : 400,
                          border: isCurrentDay ? "2px solid #3b82f6" : "none",
                        }}
                      >
                        {date ? format(date, "d") : ""}
                      </div>
                    );
                  })}
                </div>
              </div>

              {/* Quick Actions */}
              <div>
                <h3
                  className="mb-3"
                  style={{ fontSize: "1.125rem", fontWeight: 600 }}
                >
                  Quick Actions
                </h3>
                <div className="d-flex flex-column gap-2">
                  {data.quickActions.map((action) => {
                    const iconMap: Record<string, any> = {
                      assignment: FileText,
                      document: FileText,
                      certificate: Award,
                    };
                    const Icon = iconMap[action.icon] || FileText;

                    const colorMap: Record<string, string> = {
                      purple: "#a78bfa",
                      blue: "#60a5fa",
                      green: "#34d399",
                    };

                    return (
                      <div
                        key={action.id}
                        style={{
                          border: "1px solid #e5e7eb",
                          borderRadius: "8px",
                          padding: "0.75rem",
                          cursor: "pointer",
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                        }}
                      >
                        <div className="d-flex align-items-center gap-3">
                          <div
                            style={{
                              width: "40px",
                              height: "40px",
                              borderRadius: "8px",
                              backgroundColor: colorMap[action.color],
                              display: "flex",
                              alignItems: "center",
                              justifyContent: "center",
                              color: "white",
                            }}
                          >
                            <Icon size={20} />
                          </div>
                          <span style={{ fontWeight: 500 }}>
                            {action.label}
                          </span>
                        </div>
                        <MoreHorizontal
                          size={20}
                          style={{ color: "#9ca3af" }}
                        />
                      </div>
                    );
                  })}
                </div>
              </div>
            </CCardBody>
          </CCard>
        </CCol>
      </CRow>
    </div>
  );
}
