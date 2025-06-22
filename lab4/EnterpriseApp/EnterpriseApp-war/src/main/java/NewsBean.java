import jakarta.annotation.Resource;
import jakarta.enterprise.context.RequestScoped;
import jakarta.inject.Inject;
import jakarta.inject.Named;
import jakarta.jms.*;
import lab.ejb.NewsItem;
import lab.ejb.NewsItemFacadeLocal;



import java.io.Serializable;
import java.util.List;

@RequestScoped
@Named
public class NewsBean implements Serializable {

    @Inject
    private NewsItemFacadeLocal facade;

    @Inject
    private JMSContext context;

    @Resource(lookup="java:app/jms/NewsQueue")
    private jakarta.jms.Queue queue;
    private String headingText;
    private String bodyText;

    public String sendMessage(String heading, String body) {
        try {
            // Tworzymy wiadomość tekstową zawierającą nagłówek i treść
            // oddzielone pionową kreską "|"
            String messageContent = heading + "|" + body;

            TextMessage message = context.createTextMessage(messageContent);

            // Wysyłamy wiadomość do kolejki
            context.createProducer().send(queue, message);

            // (opcjonalnie) Informujemy użytkownika na stronie, że wysłano wiadomość
            // np. przez FacesContext.getCurrentInstance().addMessage(...)
        } catch (JMSRuntimeException e) {  // lub catch (Exception e)
            e.printStackTrace();
        }
        // Zwracamy null, aby pozostać na tej samej stronie (lub nazwę innej strony)
        return null;
    }

    public String submitNews() {
        sendMessage(headingText, bodyText);
        return null;
    }

    public List<NewsItem> getNewsItems()
    {
        return facade.getAllNewsItems();
    }

    public String getHeadingText() {
        return headingText;
    }

    public void setHeadingText(String headingText) {
        this.headingText = headingText;
    }

    public String getBodyText() {
        return bodyText;
    }

    public void setBodyText(String bodyText) {
        this.bodyText = bodyText;
    }
}
