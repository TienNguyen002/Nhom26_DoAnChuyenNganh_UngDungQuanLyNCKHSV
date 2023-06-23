import axios from "axios";

export async function get_api(your_api) {
  try {
    const response = await axios.get(your_api);
    const data = response.data;
    if (data.isSuccess) return data.result;
    else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export async function post_api(your_api, formData) {
  try {
    const response = await axios({
      method: "post",
      url: your_api,
      data: formData,
      headers: {
        accept: "multipart/form-data",
        "Content-Type": "multipart/form-data",
      },
    });
    const data = response.data;
    console.log("postdata", data);
    sessionStorage.setItem("EData", data.errors);
    if (data.isSuccess) {
      sessionStorage.removeItem("EData");
      localStorage.setItem("Token", data.result.token)
      localStorage.setItem("UrlSlug", data.result.urlSlug)
      localStorage.setItem("TokenAdmin", data.result)
      return data.result;
    } else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export async function put_api(your_api, formData) {
  try {
    let formDataObject = Object.fromEntries(formData.entries());
    // Format the plain form data as JSON
    let formDataJsonString = JSON.stringify(formDataObject);

    const response = await axios({
      method: "put",
      url: your_api,
      data: formDataJsonString,
      headers: {
        accept: "application/json",
        "Content-Type": "application/json",
      },
    });

    const data = response.data;
    if (data.isSuccess) {
      return data.result;
    } else {
      return null;
    }
  } catch (error) {
    console.log("Error ", error.message);
    return null;
  }
}

export async function post_image_api(your_api, formData) {
  try {
    const response = await axios({
      method: "post",
      url: your_api,
      data: formData,
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    console.log(response);
    const data = response.data;
    if (data.isSuccess) return data.result;
    else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export async function delete_api(your_api) {
  try {
    const response = await axios.delete(your_api);
    const data = response.data;
    if (data.isSuccess) return data.result;
    else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export function decode(str) {
  let txt = new DOMParser().parseFromString(str, "text/html");
  return txt.documentElement.textContent;
}
