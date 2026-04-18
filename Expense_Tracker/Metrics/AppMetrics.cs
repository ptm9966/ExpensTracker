using Prometheus;

namespace Expense_Tracker.Observability
{
    public static class AppMetrics
    {
        public static readonly Counter TransactionWrites = Prometheus.Metrics
            .CreateCounter(
                "expense_tracker_transaction_writes_total",
                "Number of successful transaction create, update, and delete operations.",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "category_type" }
                });

        public static readonly Histogram TransactionAmountRecorded = Prometheus.Metrics
            .CreateHistogram(
                "expense_tracker_transaction_amount",
                "Distribution of successfully saved transaction amounts.",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "category_type" },
                    Buckets = Histogram.LinearBuckets(start: 0, width: 1000, count: 10)
                });

        public static readonly Counter CategoryWrites = Prometheus.Metrics
            .CreateCounter(
                "expense_tracker_category_writes_total",
                "Number of successful category create, update, and delete operations.",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "category_type" }
                });

        public static readonly Counter DashboardRequests = Prometheus.Metrics
            .CreateCounter(
                "expense_tracker_dashboard_requests_total",
                "Number of dashboard page requests.");

        public static readonly Histogram DashboardLoadDuration = Prometheus.Metrics
            .CreateHistogram(
                "expense_tracker_dashboard_load_duration_seconds",
                "Time spent building the dashboard page.",
                new HistogramConfiguration
                {
                    Buckets = Histogram.ExponentialBuckets(start: 0.01, factor: 2, count: 10)
                });

        public static readonly Gauge DashboardIncomeAmount = Prometheus.Metrics
            .CreateGauge(
                "expense_tracker_dashboard_income_amount",
                "Total income shown on the dashboard for the last seven days.");

        public static readonly Gauge DashboardExpenseAmount = Prometheus.Metrics
            .CreateGauge(
                "expense_tracker_dashboard_expense_amount",
                "Total expense shown on the dashboard for the last seven days.");

        public static readonly Gauge DashboardBalanceAmount = Prometheus.Metrics
            .CreateGauge(
                "expense_tracker_dashboard_balance_amount",
                "Net balance shown on the dashboard for the last seven days.");

        public static readonly Gauge DashboardSelectedTransactionCount = Prometheus.Metrics
            .CreateGauge(
                "expense_tracker_dashboard_selected_transactions",
                "Number of transactions used to build the current dashboard snapshot.");

        public static readonly Counter TransactionListRequests = Prometheus.Metrics
            .CreateCounter(
                "expense_tracker_transaction_list_requests_total",
                "Number of transaction list page requests.");

        public static readonly Counter CategoryListRequests = Prometheus.Metrics
            .CreateCounter(
                "expense_tracker_category_list_requests_total",
                "Number of category list page requests.");
    }
}
