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
  
    const handleSubmit = async (e: React.FormEvent) => {
      e.preventDefault();
    
      setCollabResults(searchType === 'user' ? collabResults : []);
      setContentResults(searchType === 'user' ? contentResults : []);
      setAzureResults(searchType === 'item' ? azureResults : []);
    
      if (searchType === 'user') {
        try {
          const response = await fetch('https://localhost:5000/api/AzureRecommender', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ userId: userId }),
          });
    
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
    
          const data = await response.json();
          const rawResult = data.Results.WebServiceOutput0[0];
    
          const extractedResults = [
            rawResult["Recommended Item 1"],
            rawResult["Recommended Item 2"],
            rawResult["Recommended Item 3"],
            rawResult["Recommended Item 4"],
            rawResult["Recommended Item 5"]
          ];
    
          setAzureResults(extractedResults);
        } catch (error) {
          console.error('Azure ML API call failed:', error);
          setAzureResults([]);
        }
      } else {
        setAzureResults([]);
      }

      if (searchType === 'item') {
        try {
          const collabRes = await fetch(`https://localhost:5000/api/content/collaborative/${userId}`);
          if (!collabRes.ok) throw new Error("Collaborative filtering failed.");
          const collabData = await collabRes.json();
          setCollabResults(collabData);
        } catch (err) {
          console.error("Collaborative filtering error:", err);
          setCollabResults([]);
        }
    
        try {
          const contentRes = await fetch(`https://localhost:5000/api/content/content/${userId}`);
          if (!contentRes.ok) throw new Error("Content filtering failed.");
          const contentData = await contentRes.json();
          setContentResults(contentData);
        } catch (err) {
          console.error("Content filtering error:", err);
          setContentResults([]);
        }
      }
    };
    
    
  
    return (
      <div className="container mt-5">
        <h1 className="mb-4 text-center">Recommender Project</h1>
        <p>Search by User ID to use the Azure ML Endpoint. Search by Item ID to perform Collaborative Filtering and Content Filtering.</p>
  
        <form onSubmit={handleSubmit} className="mb-5">
          <div className="text-center mb-3">
            <div className="btn-group" role="group" aria-label="Search Type">
              <input
                type="radio"
                className="btn-check"
                name="searchType"
                id="userSearch"
                checked={searchType === 'user'}
                onChange={() => setSearchType('user')}
              />
              <label className="btn btn-outline-primary" htmlFor="userSearch">
                Search by User ID
              </label>
  
              <input
                type="radio"
                className="btn-check"
                name="searchType"
                id="itemSearch"
                checked={searchType === 'item'}
                onChange={() => setSearchType('item')}
              />
              <label className="btn btn-outline-primary" htmlFor="itemSearch">
                Search by Item ID
              </label>
            </div>
          </div>
  
          <div className="row justify-content-center">
            <div className="col-md-6">
              <label htmlFor="userId" className="form-label">
                Enter {searchType === 'user' ? 'User' : 'Item'} ID:
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