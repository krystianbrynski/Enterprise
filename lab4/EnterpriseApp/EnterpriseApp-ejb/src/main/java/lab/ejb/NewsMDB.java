package lab.ejb;

import jakarta.ejb.ActivationConfigProperty;
import jakarta.ejb.MessageDriven;
import jakarta.inject.Inject;
import jakarta.jms.*;
import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;

@JMSDestinationDefinition(name = "java:app/jms/NewsQueue",
        interfaceName = "jakarta.jms.Queue", resourceAdapter = "jmsra",
        destinationName = "NewsQueue")
@MessageDriven(activationConfig = {
        @ActivationConfigProperty(propertyName =
                "destinationLookup", propertyValue = "java:app/jms/NewsQueue"),
        @ActivationConfigProperty(propertyName = "destinationType",
                propertyValue = "jakarta.jms.Queue")
})
public class NewsMDB implements jakarta.jms.MessageListener {

    @PersistenceContext
    private EntityManager em;

    @Inject
    private NewsItemFacadeLocal facade;

    @Override
    public void onMessage(Message message) {
        if (message instanceof TextMessage) {
            TextMessage textMessage = (TextMessage) message;
            try {
                // Odczytujemy treść wiadomości, np. "Nagłówek|Treść newsa"
                String content = textMessage.getText();
                // Dzielimy tekst na nagłówek i treść newsa (rozdzielamy według pionowej kreski)
                String[] parts = content.split("\\|", 2);
                if (parts.length < 2) {
                    System.err.println("Błędny format wiadomości: " + content);
                    return;
                }
                String heading = parts[0];
                String body = parts[1];

                // Tworzymy nowy obiekt encji NewsItem i ustawiamy wartości
                NewsItem newsItem = new NewsItem();
                newsItem.setHeading(heading);
                newsItem.setBody(body);

                // Utrwalamy encję w bazie danych za pomocą fasady
                facade.create(newsItem);

                System.out.println("NewsItem utworzony: " + newsItem);
            } catch (JMSException e) {
                e.printStackTrace();
            }
        } else {
            System.err.println("Otrzymano nieobsługiwany typ wiadomości: " + message.getClass());
        }
    }
}
