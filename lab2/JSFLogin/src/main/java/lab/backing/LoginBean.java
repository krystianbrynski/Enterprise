package lab.backing;


import jakarta.enterprise.context.RequestScoped;
import jakarta.faces.component.html.HtmlCommandButton;
import jakarta.faces.component.html.HtmlSelectBooleanCheckbox;
import jakarta.faces.event.AjaxBehaviorEvent;
import jakarta.faces.event.ValueChangeEvent;
import jakarta.inject.Named;
import jakarta.faces.context.FacesContext;

@RequestScoped
@Named
public class LoginBean {
    private String username;
    private String password;
    private HtmlSelectBooleanCheckbox acceptCheckBox;
    private HtmlCommandButton loginButton;

    public String login() {
        if (username.equals(password)) {
            return "success";
        } else {
            return "failure";
        }
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public HtmlSelectBooleanCheckbox getAcceptCheckBox() {
        return acceptCheckBox;
    }

    public void setAcceptCheckBox(HtmlSelectBooleanCheckbox acceptCheckBox) {
        this.acceptCheckBox = acceptCheckBox;
    }

    public HtmlCommandButton getLoginButton() {
        return loginButton;
    }

    public void setLoginButton(HtmlCommandButton loginButton) {
        this.loginButton = loginButton;
    }

    public void activateButton(AjaxBehaviorEvent event) {
        if (acceptCheckBox.isSelected()) {
            loginButton.setDisabled(false);
        } else {
            loginButton.setDisabled(true);
        }
        FacesContext context = FacesContext.getCurrentInstance();
        context.renderResponse();
    }

}
