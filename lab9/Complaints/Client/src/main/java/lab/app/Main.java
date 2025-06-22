package lab.app;

import jakarta.ws.rs.client.Client;
import jakarta.ws.rs.client.ClientBuilder;
import jakarta.ws.rs.core.MediaType;

public class Main {
    public static void main(String[] args) {
        Client client = ClientBuilder.newClient();
        String status = client.target("http://localhost:8080/Server-1.0-SNAPSHOT/api/complaints/201/status")
                .request(MediaType.TEXT_PLAIN)
                .get(String.class);
        System.out.println("Status: " + status);
        client.close();
    }
}
