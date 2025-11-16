interface SearchFiltersProps {
  faculty: string;
  setFaculty: (value: string) => void;
  role: string; 
  setRole: (value: string) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
  onAddUser: () => void;
}

export default function SearchFilters({
  faculty,
  setFaculty,
  role,
  setRole,
  searchTerm,
  setSearchTerm,
  onAddUser,
}: SearchFiltersProps) {
  return (
    <div className="bg-white rounded-lg shadow p-6">
      <div className="grid grid-cols-2 gap-6 mb-6">
        <div>
          <label className="block text-sm font-semibold mb-2">Faculty</label>
          <select
            value={faculty}
            onChange={(e) => setFaculty(e.target.value)}
            className="w-full px-4 py-2 border border-gray-300 rounded bg-white"
          >
            <option>ETF UNSA</option>
            <option>All</option>
          </select>
        </div>
        <div>
          <label className="block text-sm font-semibold mb-2">Role</label>
          <select
            value={role}
            onChange={(e) => setRole(e.target.value)}
            className="w-full px-4 py-2 border border-gray-300 rounded bg-white"
          >
            <option value="All">All</option>
            <option>Professor</option>
            <option>Assistant</option>
            <option>Student</option>
            <option>Staff</option>
          </select>
        </div>
      </div>

      <input
        type="text"
        placeholder="Search for user.."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        className="w-full px-4 py-2 border border-gray-300 rounded mb-6 text-sm"
      />

      <div className="flex gap-3 justify-center">
        <button className="bg-blue-900 text-white px-6 py-2 rounded text-sm font-semibold hover:bg-blue-800">
          Search
        </button>
        <button
          onClick={onAddUser}
          className="bg-blue-900 text-white px-6 py-2 rounded text-sm font-semibold hover:bg-blue-800"
        >
          + Add User
        </button>
        <button className="bg-blue-900 text-white px-6 py-2 rounded text-sm font-semibold hover:bg-blue-800">
          + Add in bulk
        </button>
      </div>
    </div>
  );
}
