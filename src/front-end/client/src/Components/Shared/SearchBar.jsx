import { useState } from "react";
import Form from 'react-bootstrap/Form'
import  Button  from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { FormControl } from "react-bootstrap";

const SearchBar = () => {
    const [keyword, setKeyword] = useState('');
  
    const handleSubmit = (e) => {
      e.preventDefault();
      window.location = `/k=${keyword}`;
    }
  
    return(
      <div className="mb-4">
        <Form method="get" onSubmit={handleSubmit}>
          <Form.Group className="input-group mb-3">
            <FormControl
            type="text"
            name="k"
            value={keyword}
            onChange = {(e) => setKeyword(e.target.value)}
            aria-label = 'Nhập từ khóa cần tìm'
            aria-describedby="btnSearchPost"
            placeholder="Nhập từ khóa cần tìm"/>

            <Button
            id='btnSearchPost'
            variant = 'outline-secondary'
            type='submit'
            className="search-button">
              <FontAwesomeIcon icon={faSearch}/>
            </Button>
          </Form.Group>
        </Form>
      </div>
    )
  }
  
export default SearchBar;