package lab.requests.backing;

import jakarta.enterprise.context.RequestScoped;
import jakarta.inject.Inject;
import jakarta.inject.Named;
import jakarta.validation.constraints.Size;
import lab.requests.data.RequestRepository;
import lab.requests.entities.Request;
import java.time.LocalDate;
import java.util.List;
import jakarta.transaction.Transactional;

@RequestScoped
@Named
public class RequestsList {
    @Inject
    private RequestRepository requestRepository;
    @Size(min = 3, max = 60, message = "{request.size}")
    private String newRequest;
    private jakarta.faces.component.html.HtmlDataTable requestsDataTable;
    public List<Request> getAllRequests() {
        return requestRepository.findAll();
    }

    public String getNewRequest() {
        return newRequest;
    }

    public void setNewRequest(String newRequest) {
        this.newRequest = newRequest;
    }

    @Transactional
    public String addRequest()
    {
        Request request = new Request();
        request.setRequestText(getNewRequest());
        request.setRequestDate(LocalDate.now());
        requestRepository.create(request);
        setNewRequest("");
        return "requestsList?faces-redirect=true";
    }

    @Transactional
    public String deleteRequest() {
        Request req = (Request) getRequestsDataTable().getRowData();
        requestRepository.remove(req);
        return "requestsList?faces-redirect=true";
    }

    public jakarta.faces.component.html.HtmlDataTable getRequestsDataTable() {
        return requestsDataTable;
    }

    public void setRequestsDataTable(jakarta.faces.component.html.HtmlDataTable requestsDataTable) {
        this.requestsDataTable = requestsDataTable;
    }
}