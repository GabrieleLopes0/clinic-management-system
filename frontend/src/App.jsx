import { useEffect, useMemo, useState } from 'react';

const API_URL = 'http://localhost:5000/api';

function App() {
  const [token, setToken] = useState(localStorage.getItem('clinic_token') || '');
  const [view, setView] = useState(token ? 'patients' : 'login');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [email, setEmail] = useState('admin@clinic.com');
  const [password, setPassword] = useState('123456');

  const [patients, setPatients] = useState([]);
  const [professionals, setProfessionals] = useState([]);
  const [patientName, setPatientName] = useState('');
  const [patientEmail, setPatientEmail] = useState('');
  const [patientBirthDate, setPatientBirthDate] = useState('');
  const [professionalName, setProfessionalName] = useState('');
  const [professionalSpecialty, setProfessionalSpecialty] = useState('');

  const [selectedPatientId, setSelectedPatientId] = useState('');
  const [selectedProfessionalId, setSelectedProfessionalId] = useState('');
  const [appointmentDate, setAppointmentDate] = useState('');
  const [agenda, setAgenda] = useState([]);

  const isAuthenticated = useMemo(() => !!token, [token]);

  useEffect(() => {
    if (isAuthenticated) {
      loadPatients();
      loadProfessionals();
    }
  }, [isAuthenticated]);

  useEffect(() => {
    if (selectedProfessionalId) {
      loadAgenda(selectedProfessionalId);
    }
  }, [selectedProfessionalId]);

  const headers = {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {})
  };

  async function request(path, options = {}) {
    setError('');
    setLoading(true);
    try {
      const response = await fetch(`${API_URL}${path}`, {
        ...options,
        headers: { ...headers, ...(options.headers || {}) }
      });

      if (!response.ok) {
        const body = await response.json().catch(() => null);
        throw new Error(body?.error || 'Ocorreu um erro na requisição');
      }

      if (response.status === 204) {
        return null;
      }
      return await response.json();
    } finally {
      setLoading(false);
    }
  }

  async function handleLogin(event) {
    event.preventDefault();
    const result = await request('/auth/login', {
      method: 'POST',
      body: JSON.stringify({ email, password })
    });

    setToken(result.accessToken);
    localStorage.setItem('clinic_token', result.accessToken);
    setView('patients');
  }

  async function loadPatients() {
    const data = await request('/patients');
    setPatients(data);
  }

  async function loadProfessionals() {
    const data = await request('/professionals');
    setProfessionals(data);
  }

  async function handleCreatePatient(event) {
    event.preventDefault();
    await request('/patients', {
      method: 'POST',
      body: JSON.stringify({ name: patientName, email: patientEmail, birthDate: patientBirthDate })
    });
    setPatientName('');
    setPatientEmail('');
    setPatientBirthDate('');
    loadPatients();
  }

  async function handleCreateProfessional(event) {
    event.preventDefault();
    await request('/professionals', {
      method: 'POST',
      body: JSON.stringify({ name: professionalName, specialty: professionalSpecialty })
    });
    setProfessionalName('');
    setProfessionalSpecialty('');
    loadProfessionals();
  }

  async function loadAgenda(professionalId) {
    const data = await request(`/appointments/professional/${professionalId}`);
    setAgenda(data);
  }

  function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' });
  }

  async function handleCreateAppointment(event) {
    event.preventDefault();
    const localDate = new Date(appointmentDate);
    const utcDate = new Date(localDate.getTime() - localDate.getTimezoneOffset() * 60000).toISOString();

    await request('/appointments', {
      method: 'POST',
      body: JSON.stringify({
        patientId: selectedPatientId,
        professionalId: selectedProfessionalId,
        appointmentDate: utcDate
      })
    });

    setAppointmentDate('');
    loadAgenda(selectedProfessionalId);
  }

  function logout() {
    setToken('');
    localStorage.removeItem('clinic_token');
    setView('login');
  }

  if (!isAuthenticated) {
    return (
      <div className="app-container">
        <h1>Login</h1>
        <form onSubmit={handleLogin} className="card">
          <label>e-mail</label>
          <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" />
          <label>senha</label>
          <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" />
          <button type="submit" disabled={loading}>Entrar</button>
        </form>
        {error && <div className="error">{error}</div>}
      </div>
    );
  }

  return (
    <div className="app-container">
      <header>
        <div>
          <button className={view === 'patients' ? 'active' : ''} onClick={() => setView('patients')}>Pacientes</button>
          <button className={view === 'professionals' ? 'active' : ''} onClick={() => setView('professionals')}>Profissionais</button>
          <button className={view === 'appointments' ? 'active' : ''} onClick={() => setView('appointments')}>Consultas</button>
        </div>
        <button className="logout" onClick={logout}>Sair</button>
      </header>

      {error && <div className="error">{error}</div>}

      {view === 'patients' && (
        <section className="card">
          <h2>Pacientes</h2>
          <form onSubmit={handleCreatePatient} className="form-grid">
            <div>
              <label>Nome</label>
              <input value={patientName} onChange={(e) => setPatientName(e.target.value)} required />
            </div>
            <div>
              <label>E-mail</label>
              <input value={patientEmail} onChange={(e) => setPatientEmail(e.target.value)} type="email" required />
            </div>
            <div>
              <label>Data de nascimento</label>
              <input value={patientBirthDate} onChange={(e) => setPatientBirthDate(e.target.value)} type="date" required />
            </div>
            <button type="submit" disabled={loading}>Cadastrar paciente</button>
          </form>

          <div className="table-wrap">
            <table>
              <thead>
                <tr><th>Nome</th><th>E-mail</th><th>Nascimento</th></tr>
              </thead>
              <tbody>
                {patients.map((patient) => (
                  <tr key={patient.id}>
                    <td>{patient.name}</td>
                    <td>{patient.email}</td>
                    <td>{new Date(patient.birthDate).toLocaleDateString('pt-BR')}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      )}

      {view === 'professionals' && (
        <section className="card">
          <h2>Profissionais</h2>
          <form onSubmit={handleCreateProfessional} className="form-grid">
            <div>
              <label>Nome</label>
              <input value={professionalName} onChange={(e) => setProfessionalName(e.target.value)} required />
            </div>
            <div>
              <label>Especialidade</label>
              <input value={professionalSpecialty} onChange={(e) => setProfessionalSpecialty(e.target.value)} required />
            </div>
            <button type="submit" disabled={loading}>Cadastrar profissional</button>
          </form>
          <div className="table-wrap">
            <table>
              <thead>
                <tr><th>Nome</th><th>Especialidade</th></tr>
              </thead>
              <tbody>
                {professionals.map((professional) => (
                  <tr key={professional.id}>
                    <td>{professional.name}</td>
                    <td>{professional.specialty}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>
      )}

      {view === 'appointments' && (
        <section className="card">
          <h2>Agendamento de consultas</h2>
          <form onSubmit={handleCreateAppointment} className="form-grid">
            <div>
              <label>Paciente</label>
              <select value={selectedPatientId} onChange={(e) => setSelectedPatientId(e.target.value)} required>
                <option value="">Selecione</option>
                {patients.map((patient) => (
                  <option key={patient.id} value={patient.id}>{patient.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label>Profissional</label>
              <select value={selectedProfessionalId} onChange={(e) => setSelectedProfessionalId(e.target.value)} required>
                <option value="">Selecione</option>
                {professionals.map((professional) => (
                  <option key={professional.id} value={professional.id}>{professional.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label>Data / hora</label>
              <input value={appointmentDate} onChange={(e) => setAppointmentDate(e.target.value)} type="datetime-local" required />
            </div>
            <button type="submit" disabled={loading || !selectedProfessionalId || !selectedPatientId}>Agendar</button>
          </form>

          {selectedProfessionalId && (
            <div className="table-wrap">
              <h3>Agenda do profissional</h3>
              <table>
                <thead>
                  <tr><th>Paciente</th><th>Data / hora</th></tr>
                </thead>
                <tbody>
                  {agenda.map((item) => (
                    <tr key={item.id}>
                      <td>{item.patientName}</td>
                      <td>{formatDate(item.appointmentDate)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </section>
      )}
    </div>
  );
}

export default App;
