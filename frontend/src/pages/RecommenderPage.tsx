import { useState } from "react";

const RecommenderPage = () => {
    const [searchType, setSearchType] = useState<'user' | 'item'>('user');
    const [userId, setUserId] = useState('');
    const [collabResults, setCollabResults] = useState<string[]>([]);
    const [contentResults, setContentResults] = useState<string[]>([]);
    const [azureResults, setAzureResults] = useState<string[]>([]);
  
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      setUserId(e.target.value);
    };
  
    const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
  
      // Sample pseudo data
      const fakeCollab = ['C101', 'C205', 'C312', 'C487', 'C509'];
      const fakeContent = ['T102', 'T215', 'T318', 'T403', 'T555'];
      const fakeAzure = ['A103', 'A204', 'A319', 'A499', 'A622'];
  
      setCollabResults(fakeCollab);
      setContentResults(fakeContent);
      setAzureResults(fakeAzure);
    };
  
    return (
      <div className="container mt-5">
        <h1 className="mb-4 text-center">Recommender Project</h1>
  
        <form onSubmit={handleSubmit} className="mb-5">
          <div className="row justify-content-center">
            <div className="col-md-6">
              <label htmlFor="userId" className="form-label">
                Enter Item ID:
              </label>
              <input
                type="text"
                id="userId"
                className="form-control"
                value={userId}
                onChange={handleInputChange}
                required
              />
            </div>
          </div>
          <div className="text-center mt-3">
            <button type="submit" className="btn btn-primary">
              Get Recommendations
            </button>
          </div>
        </form>
  
        <div className="row">
          <div className="col-md-4">
            <h4>Collaborative Filtering</h4>
            <ul className="list-group">
              {collabResults.map((id, index) => (
                <li key={index} className="list-group-item">Item ID: {id}</li>
              ))}
            </ul>
          </div>
          <div className="col-md-4">
            <h4>Content Filtering</h4>
            <ul className="list-group">
              {contentResults.map((id, index) => (
                <li key={index} className="list-group-item">Item ID: {id}</li>
              ))}
            </ul>
          </div>
          <div className="col-md-4">
            <h4>Azure ML Endpoint</h4>
            <ul className="list-group">
              {azureResults.map((id, index) => (
                <li key={index} className="list-group-item">Item ID: {id}</li>
              ))}
            </ul>
          </div>
        </div>
      </div>
    );
  };
  
  export default RecommenderPage;