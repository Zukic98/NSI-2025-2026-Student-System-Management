"use client"

import { useState, useEffect } from "react"
import { CButton, CModal, CModalHeader, CModalTitle, CModalBody, CModalFooter } from "@coreui/react"
import mockAPI from "./mockApi"
import AssignmentForm from "./AssignmentForm"
import styles from "./AssignmentManagement.module.css"
import { useAPI } from "../../context/services"

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

interface CreateAssignmentDTO {
  course: string
  name: string
  description: string
  dueDate: Date
  maxPoints: number
}

export default function AssignmentManagement() {
  // Form state for creating assignments
  const [course, setCourse] = useState("")
  const [name, setName] = useState("")
  const [description, setDescription] = useState("")
  const [dueDate, setDueDate] = useState<Date | null>(null)
  const [maxPoints, setMaxPoints] = useState("")

  // State for assignments list
  const [assignments, setAssignments] = useState<Assignment[]>([])
  const [searchQuery, setSearchQuery] = useState("")

  // Edit modal state
  const [showEditModal, setShowEditModal] = useState(false)
  const [editingAssignment, setEditingAssignment] = useState<Assignment | null>(null)

  // Delete confirmation modal state
  const [showDeleteModal, setShowDeleteModal] = useState(false)
  const [deletingAssignmentId, setDeletingAssignmentId] = useState<string | null>(null)

  // Validation errors
  const [errors, setErrors] = useState<Record<string, string>>({})

  // API hook (ready for real API integration)
  const api = useAPI()

  // Load assignments on mount
  useEffect(() => {
    loadAssignments()
  }, [])

  const loadAssignments = async () => {
    try {
      // Mock API call - replace with: const data = await api.getAssignments()
      const data = await mockAPI.getAssignments()
      setAssignments(data)
    } catch (error) {
      console.error("Failed to load assignments:", error)
    }
  }

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {}

    if (!course) {
      newErrors.course = "Course is required"
    }
    if (!name.trim()) {
      newErrors.name = "Name is required"
    }
    if (!dueDate) {
      newErrors.dueDate = "Due date is required"
    } else if (dueDate <= new Date()) {
      newErrors.dueDate = "Due date must be in the future"
    }
    if (!maxPoints || Number.parseFloat(maxPoints) <= 0) {
      newErrors.maxPoints = "Max points must be a positive number"
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleCreate = async () => {
    if (!validateForm()) {
      return
    }

    const dto: CreateAssignmentDTO = {
      course,
      name,
      description,
      dueDate: dueDate!,
      maxPoints: Number.parseFloat(maxPoints),
    }

    try {
      // Mock API call - replace with: await api.createAssignment(dto)
      await mockAPI.createAssignment(dto)

      // Refresh assignments table
      await loadAssignments()

      // Reset form
      setCourse("")
      setName("")
      setDescription("")
      setDueDate(null)
      setMaxPoints("")
      setErrors({})

      // Show success feedback (you can add a toast notification here)
      alert("Assignment created successfully!")
    } catch (error) {
      console.error("Failed to create assignment:", error)
      alert("Failed to create assignment")
    }
  }

  const handleEdit = (assignment: Assignment) => {
    setEditingAssignment(assignment)
    setShowEditModal(true)
  }

  const handleSaveEdit = async () => {
    if (!editingAssignment) return

    try {
      // Mock API call - replace with: await api.updateAssignment(editingAssignment.id, editingAssignment)
      await mockAPI.updateAssignment(editingAssignment.id, editingAssignment)

      // Refresh table
      await loadAssignments()

      // Close modal
      setShowEditModal(false)
      setEditingAssignment(null)

      alert("Assignment updated successfully!")
    } catch (error) {
      console.error("Failed to update assignment:", error)
      alert("Failed to update assignment")
    }
  }

  const handleDeleteClick = (id: string) => {
    setDeletingAssignmentId(id)
    setShowDeleteModal(true)
  }

  const handleConfirmDelete = async () => {
    if (!deletingAssignmentId) return

    try {
      // Mock API call - replace with: await api.deleteAssignment(deletingAssignmentId)
      await mockAPI.deleteAssignment(deletingAssignmentId)

      // Refresh table
      await loadAssignments()

      // Close modal
      setShowDeleteModal(false)
      setDeletingAssignmentId(null)

      alert("Assignment deleted successfully!")
    } catch (error) {
      console.error("Failed to delete assignment:", error)
      alert("Failed to delete assignment")
    }
  }

  // Filter assignments based on search query
  const filteredAssignments = assignments.filter(
    (assignment) =>
      assignment.course.toLowerCase().includes(searchQuery.toLowerCase()) ||
      assignment.name.toLowerCase().includes(searchQuery.toLowerCase()),
  )

  return (
    <div className={styles.assignmentContainer}>
      <div className={styles.contentWrapper}>
        <h1 className={styles.pageTitle}>Assignment management</h1>

        {/* Create Assignments Section */}
        <div className={styles.sectionCard}>
          <div className={styles.sectionCardBody}>
            <h2 className={styles.sectionTitle}>Create Assignments</h2>
            <AssignmentForm
              formData={{ course, name, description, dueDate, maxPoints }}
              errors={errors}
              onCourseChange={setCourse}
              onNameChange={setName}
              onDescriptionChange={setDescription}
              onDueDateChange={setDueDate}
              onMaxPointsChange={setMaxPoints}
            />

            <div className={styles.buttonContainer}>
              <button onClick={handleCreate} className={styles.createButton}>
                Create
              </button>
            </div>
          </div>
        </div>

        {/* Edit Assignments Section */}
        <div className={styles.sectionCard}>
          <div className={styles.sectionCardBody}>
            <h2 className={styles.sectionTitle}>Edit Assignments</h2>

            <div className={styles.searchContainer}>
              <span className={styles.searchIcon}>
                <svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M9 17A8 8 0 1 0 9 1a8 8 0 0 0 0 16zM19 19l-4.35-4.35"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
              </span>
              <input
                type="text"
                placeholder="Search"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className={styles.searchInput}
              />
            </div>

            <div className={styles.tableContainer}>
              <table className={styles.table}>
                <thead className={styles.tableHeader}>
                  <tr>
                    <th className={styles.tableHeaderCell}>Course</th>
                    <th className={styles.tableHeaderCell}>Name</th>
                    <th className={styles.tableHeaderCell}>Faculty</th>
                    <th className={styles.tableHeaderCell}>Max points</th>
                    <th className={styles.tableHeaderCell}>Major</th>
                    <th className={`${styles.tableHeaderCell} ${styles.actionsCell}`}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredAssignments.map((assignment) => (
                    <tr key={assignment.id} className={styles.tableRow}>
                      <td className={styles.tableCell}>{assignment.course}</td>
                      <td className={styles.tableCell}>{assignment.name}</td>
                      <td className={styles.tableCell}>{assignment.faculty}</td>
                      <td className={styles.tableCell}>{assignment.maxPoints}</td>
                      <td className={styles.tableCell}>{assignment.major}</td>
                      <td className={`${styles.tableCell} ${styles.actionsCell}`}>
                        <div className={styles.actionButtonsContainer}>
                          <button
                            onClick={() => handleEdit(assignment)}
                            className={`${styles.actionButton} ${styles.editButton}`}
                            title="Edit"
                            type="button"
                          >
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"
                              />
                            </svg>
                          </button>
                          <button
                            onClick={() => handleDeleteClick(assignment.id)}
                            className={`${styles.actionButton} ${styles.deleteButton}`}
                            title="Delete"
                            type="button"
                          >
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                              <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                              />
                            </svg>
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>

      {/* Edit Modal */}
      <CModal visible={showEditModal} onClose={() => setShowEditModal(false)} size="lg" backdrop={false}>
        <CModalHeader>
          <CModalTitle>Edit Assignment</CModalTitle>
        </CModalHeader>
        <CModalBody>
          {editingAssignment && (
            <AssignmentForm
              formData={{
                course: editingAssignment.course,
                name: editingAssignment.name,
                description: editingAssignment.description,
                dueDate: editingAssignment.dueDate ? new Date(editingAssignment.dueDate) : null,
                maxPoints: editingAssignment.maxPoints,
              }}
              errors={{}}
              onCourseChange={(value) =>
                setEditingAssignment({
                  ...editingAssignment,
                  course: value,
                })
              }
              onNameChange={(value) =>
                setEditingAssignment({
                  ...editingAssignment,
                  name: value,
                })
              }
              onDescriptionChange={(value) =>
                setEditingAssignment({
                  ...editingAssignment,
                  description: value,
                })
              }
              onDueDateChange={(value) =>
                setEditingAssignment({
                  ...editingAssignment,
                  dueDate: value || new Date(),
                })
              }
              onMaxPointsChange={(value) =>
                setEditingAssignment({
                  ...editingAssignment,
                  maxPoints: Number.parseFloat(value) || 0,
                })
              }
            />
          )}
        </CModalBody>
        <CModalFooter>
          <CButton color="secondary" onClick={() => setShowEditModal(false)}>
            Cancel
          </CButton>
          <CButton color="primary" onClick={handleSaveEdit}>
            Save Changes
          </CButton>
        </CModalFooter>
      </CModal>

      {/* Delete Confirmation Modal */}
      <CModal visible={showDeleteModal} onClose={() => setShowDeleteModal(false)} backdrop={false}>
        <CModalHeader>
          <CModalTitle>Confirm Delete</CModalTitle>
        </CModalHeader>
        <CModalBody>Are you sure you want to delete this assignment? This action cannot be undone.</CModalBody>
        <CModalFooter>
          <CButton color="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </CButton>
          <CButton color="danger" onClick={handleConfirmDelete}>
            Delete
          </CButton>
        </CModalFooter>
      </CModal>
    </div>
  )
}