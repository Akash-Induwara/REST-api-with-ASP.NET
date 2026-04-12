import { useState, useEffect } from 'react';

// Updated to match your .http file port
const API_BASE = import.meta.env.VITE_API_URL;

const GameManager = () => {
    const [games, setGames] = useState([]);
    const [formData, setFormData] = useState({ title: '', genreId: 1, price: 0, releaseDate: '' });
    const [editingId, setEditingId] = useState(null);

    // 1. GET ALL GAMES
    const loadGames = async () => {
        try {
            const response = await fetch(`${API_BASE}/games`);
            if (!response.ok) throw new Error("Network response was not ok");
            const data = await response.json();
            setGames(data);
        } catch (error) {
            console.error("Error fetching games:", error);
        }
    };

    useEffect(() => { loadGames(); }, []);

    // 2. CREATE OR UPDATE
    const handleSubmit = async (e) => {
        e.preventDefault();
        const url = editingId ? `${API_BASE}/games/${editingId}` : `${API_BASE}/games`;
        const method = editingId ? 'PUT' : 'POST';

        try {
            const response = await fetch(url, {
                method: method,
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                setEditingId(null);
                setFormData({ title: '', genreId: 1, price: 0, releaseDate: '' });
                loadGames();
            }
        } catch (error) {
            console.error("Error saving game:", error);
        }
    };

    // 3. DELETE
    const deleteGame = async (id) => {
        try {
            const response = await fetch(`${API_BASE}/games/${id}`, { method: 'DELETE' });
            if (response.ok) loadGames();
        } catch (error) {
            console.error("Error deleting game:", error);
        }
    };

    // 4. PREPARE EDIT
    const startEdit = (game) => {
        setEditingId(game.id);
        setFormData({
            title: game.title,
            genreId: game.genreId,
            price: game.price,
            releaseDate: game.releaseDate
        });
    };

    return (
        <div style={{ padding: '20px', maxWidth: '800px', margin: '0 auto', fontFamily: 'Arial' }}>
            <h1>Game Store Admin (Port: 5086)</h1>
            
            {/* FORM */}
            <form onSubmit={handleSubmit} style={{ border: '1px solid #ddd', padding: '20px', borderRadius: '8px', marginBottom: '20px' }}>
                <h3>{editingId ? 'Edit Game' : 'Add New Game'}</h3>
                <div style={{ display: 'grid', gap: '10px' }}>
                    <input type="text" placeholder="Game Title" value={formData.title} onChange={e => setFormData({...formData, title: e.target.value})} required />
                    <input type="number" placeholder="Genre ID (1-5)" value={formData.genreId} onChange={e => setFormData({...formData, genreId: parseInt(e.target.value)})} required />
                    <input type="number" step="0.01" placeholder="Price" value={formData.price} onChange={e => setFormData({...formData, price: parseFloat(e.target.value)})} required />
                    <input type="date" value={formData.releaseDate} onChange={e => setFormData({...formData, releaseDate: e.target.value})} required />
                    <button type="submit" style={{ background: '#007bff', color: 'white', padding: '10px', border: 'none', cursor: 'pointer' }}>
                        {editingId ? 'Update Game' : 'Add Game'}
                    </button>
                    {editingId && <button onClick={() => setEditingId(null)} type="button">Cancel</button>}
                </div>
            </form>

            {/* TABLE */}
            <table border="1" width="100%" style={{ borderCollapse: 'collapse' }}>
                <thead>
                    <tr style={{ background: '#f4f4f4' }}>
                        <th>ID</th><th>Title</th><th>Price</th><th>Date</th><th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {games.length === 0 ? (
                        <tr><td colSpan="5" style={{ textAlign: 'center', padding: '10px' }}>No games found. Connect your backend!</td></tr>
                    ) : (
                        games.map(g => (
                            <tr key={g.id}>
                                <td style={{ padding: '10px' }}>{g.id}</td>
                                <td>{g.title}</td>
                                <td>${g.price}</td>
                                <td>{g.releaseDate}</td>
                                <td>
                                    <button onClick={() => startEdit(g)}>Edit</button>
                                    <button onClick={() => deleteGame(g.id)} style={{ color: 'red', marginLeft: '5px' }}>Delete</button>
                                </td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default GameManager;