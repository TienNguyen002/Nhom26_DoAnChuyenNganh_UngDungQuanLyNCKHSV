import "./login.scss";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

const LoginPage = () => {
    return(
        <div className="login">
            <div className="login-mainp">
                <h1 className="login-header">Đăng nhập Sinh viên</h1>
                <form action="#">
                    <input type="text" placeholder="Email" className="login-form"/>
                    <input type="password" placeholder="Mật khẩu" className="login-form"/>
                </form>
                <Button className="login-register">Đăng nhập</Button>
                <div className="login-member">
                    Chưa có tài khoản? <Link to={`/dang-ky`} className="text-decoration-none">Đăng ký ngay</Link>
                </div>
                <div className="login-member">
                    <Link to={`/dang-nhap-admin`} className="text-decoration-none">ADMIN</Link>
                </div>
                <div className="login-member">
                    <Link to={`/`} className="text-decoration-none">QUAY LẠI</Link>
                </div>
            </div>
        </div>
    )
}

export default LoginPage;