import { useState, useEffect } from 'react';

const API_URL = process.env.REACT_APP_API_URL || 'https://reactlivedev-gadcedbdb0bpctcn.westus-01.azurewebsites.net';

function App() {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await fetch(`${API_URL}/weatherforecast`); // Common default ASP.NET endpoint
      if (!response.ok) throw new Error('Failed to fetch');
      const result = await response.json();
      setData(result);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="App">
      <h1>React Live Dev</h1>
      <p>API URL: {API_URL}</p>
      {loading && <p>Loading...</p>}
      {error && <p>Error: {error}</p>}
      {data && (
        <ul>
          {data.map((item, index) => (
            <li key={index}>
              {item.date}: {item.temperatureC}°C - {item.summary}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default App;
