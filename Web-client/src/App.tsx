import AppRouter from './router/AppRouter';
import { Navigation } from './components/Navigation';

function App() {
  return (
    <div className="min-h-screen bg-slate-100">
      <Navigation />
      <main className="mx-auto w-full max-w-6xl px-4 py-6">
        <AppRouter />
      </main>
    </div>
  );
}

export default App;
