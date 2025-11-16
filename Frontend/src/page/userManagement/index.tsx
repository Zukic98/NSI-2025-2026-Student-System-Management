import { useState } from 'react';
import type { User } from '../../types/User';
import SearchFilters from '../../component/user-management/SearchFilters';
import UsersTable from '../../component/user-management/UsersTable';
import UserDetailsPanel from '../../component/user-management/UserDetailsPanel';
import AddUserModal from '../../component/user-management/modals/AddUserModal';
import DeleteConfirmDialog from '../../component/user-management/modals/DeleteUserDialog';
import Toast from '../../component/Toast';
import Header from '../../component/Header';
export default function UserManagement() {
  const [users, setUsers] = useState<User[]>([
    { id: 1, name: 'Ama Ama', email: 'ama@unsa.ba', role: 'Professor', faculty: 'ETF UNSA', lastActive: 'Today' },
    { id: 2, name: 'Laja Laja', email: 'laja@unsa.ba', role: 'Assistant', faculty: 'ETF UNSA', lastActive: '1 week ago' },
    { id: 3, name: 'John Smith', email: 'john@unsa.ba', role: 'Student', faculty: 'ETF UNSA', lastActive: 'Yesterday' },
    { id: 4, name: 'Naja Naja', email: 'naja@unsa.ba', role: 'Staff', faculty: 'ETF UNSA', lastActive: 'Today' },
  ]);

  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [toast, setToast] = useState<{ message: string; type: 'success' | 'error' } | null>(null);
  const [faculty, setFaculty] = useState('ETF UNSA');
  const [role, setRole] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');

  const showToast = (message: string, type: 'success' | 'error') => {
    setToast({ message, type });
    setTimeout(() => setToast(null), 3000);
  };

  const filteredUsers = users.filter(u => {
    const matchesFaculty = faculty === 'All' || u.faculty === faculty;
    const matchesRole = role === 'All' || u.role === role;
    const matchesSearch = u.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
                          u.email.toLowerCase().includes(searchTerm.toLowerCase());
    return matchesFaculty && matchesRole && matchesSearch;
  });

  const handleAddUser = (newUser: Omit<User, 'id'>) => {
    const user: User = { ...newUser, id: Math.max(...users.map(u => u.id), 0) + 1 };
    setUsers([...users, user]);
    setShowAddModal(false);
    showToast('User added successfully', 'success');
  };

  const handleEditUser = (updatedUser: User) => {
    setUsers(users.map(u => u.id === updatedUser.id ? updatedUser : u));
    setSelectedUser(updatedUser);
    setShowEditModal(false);
    showToast('User updated successfully', 'success');
  };

  const handleDeleteUser = () => {
    if (selectedUser) {
      setUsers(users.filter(u => u.id !== selectedUser.id));
      setSelectedUser(null);
      setShowDeleteDialog(false);
      showToast('User deleted successfully', 'success');
    }
  };

  return (
    <div className="flex h-screen bg-gray-100">
      <div className="flex-1 flex flex-col">
       <Header /> 
        <div className="flex-1 flex gap-6 p-6 overflow-hidden">
          <div className="flex-1 flex flex-col gap-6 overflow-auto">
            <SearchFilters
              faculty={faculty}
              setFaculty={setFaculty}
              role={role}
              setRole={setRole}
              searchTerm={searchTerm}
              setSearchTerm={setSearchTerm}
              onAddUser={() => setShowAddModal(true)}
            />
            <UsersTable
              users={filteredUsers}
              selectedUser={selectedUser}
              onSelectUser={setSelectedUser}
              onEdit={() => setShowEditModal(true)}
              onDelete={() => setShowDeleteDialog(true)}
            />
          </div>
          <UserDetailsPanel user={selectedUser} />
        </div>
      </div>

      {showAddModal && (
        <AddUserModal
          onClose={() => setShowAddModal(false)}
          onAdd={handleAddUser}
        />
      )}
      {showEditModal && selectedUser && (
        <AddUserModal
          onClose={() => setShowEditModal(false)}
          onAdd={handleEditUser}
        />
      )}
      {showDeleteDialog && (
        <DeleteConfirmDialog
          user={selectedUser}
          onConfirm={handleDeleteUser}
          onCancel={() => setShowDeleteDialog(false)}
        />
      )}
      {toast && <Toast message={toast.message} type={toast.type} />}
    </div>
  );
}
